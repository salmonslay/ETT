using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Core : MonoBehaviourPun
{
    public Card[] Cards; //One of each
    public Card[] FullDeck = new Card[108]; //Contains exactly as many cards as it should
    public OwnStack Stack;
    public List<string> PlayerList = new List<string>();
    public int playerCount;
    public GameCore GC;

    

    /// <summary>
    /// Starts pre-loading the game and cards.
    /// </summary>
    private void Start()
    {
        GC = GetComponent<GameCore>();
        photonView.RPC("AddPlayer", RpcTarget.MasterClient, PhotonNetwork.NickName);
        GameObject.Find("Canvas/StartButton").GetComponent<Button>().interactable = PhotonNetwork.IsMasterClient;
        Stopwatch st = new Stopwatch();
        st.Start();

        int j = 0;
        foreach (Card Card in Cards)
        {
            for (int i = 0; i < Card.perGame; i++)
            {
                Card.InitCard();
                FullDeck[j] = Card;
                j++;
            }
        }
        st.Stop();
        Debug.Log($"Core/Start: Cards initiated. Took {st.ElapsedMilliseconds}ms to execute.");
        foreach(GameObject c in GameObject.FindGameObjectsWithTag("MyCard")) c.transform.localScale = new Vector3(c.transform.localScale.x, 0, 0.0001f);
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("PlayerName")) t.GetComponent<Text>().text = "";
    }
    /// <summary>
    /// Get a card object from ID
    /// </summary>
    public Card CardFromID(string id)
    {
        foreach (Card c in Cards)
        {
            if (c.ID == id) return c;
        }
        return null;
    }

    /// <summary>
    /// Creates a deck for everyone
    /// </summary>
    [PunRPC]
    private void ConfigureGame()
    {
        GC.SortPlayerList();
        StartCoroutine(Stack.AddCards(7));
        Destroy(GameObject.Find("Canvas/StartButton"));

        
    }

    /// <summary>
    /// Runs on master client in lobbies
    /// </summary>
    [PunRPC]
    public void AddPlayer(string name)
    {
        PlayerList.Add(name);
        photonView.RPC("DownloadPlayerlist", RpcTarget.All, string.Join("#", PlayerList.ToArray()));
    }

    /// <summary>
    /// Runs when game starts
    /// </summary>
    [PunRPC]
    public void DownloadPlayerlist(string namelist)
    {
        string[] Players = namelist.Split('#');
        GameObject.Find("Canvas/Players").GetComponent<Text>().text = $"Room: <b>{PhotonNetwork.CurrentRoom.Name}</b>\nPlayers ({Players.Length}):\n- <b>{string.Join("</b>\n- <b>", Players)}</b>";

        Stack.myID = Array.IndexOf(Players, PhotonNetwork.NickName);
        playerCount = Players.Length;
        PlayerList = Players.ToList();
    }
    #region Master
    /// <summary>
    /// Starts the game for everyone
    /// </summary>
    public void StartButton()
    {
        photonView.RPC("ConfigureGame", RpcTarget.All);

        photonView.RPC("DownloadPlayerlist", RpcTarget.All, string.Join("#", PlayerList.ToArray()));
    }

    #endregion
}