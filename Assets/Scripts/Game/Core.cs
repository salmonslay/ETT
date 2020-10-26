using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Core : MonoBehaviourPun
{
    public Card[] Cards; //One of each
    public Card[] FullDeck = new Card[108]; //Contains exactly as many cards as it should
    public List<Card> MyDeck = new List<Card>();
    public OwnStack Stack;

   public static Core Instance;

    /// <summary>
    /// Starts pre-loading the game and cards.
    /// </summary>
    private void Start()
    {
        Stopwatch st = new Stopwatch();
        st.Start();
        Instance = this;
        int j = 0;
        foreach(Card Card in Cards)
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
    }
    /// <summary>
    /// Creates a deck for everyone
    /// </summary>
    [PunRPC]
    private void ConfigureGame()
    {
        for (int i = 0; i < 7; i++)
        {
            MyDeck.Add(FullDeck[UnityEngine.Random.Range(0, 108)]);
        }
        Stack.UpdateStack();
    }
    /// <summary>
    /// Called by master client.
    /// Creates a
    /// </summary>
    public void StartButton()
    {
        photonView.RPC("ConfigureGame", RpcTarget.All);
        Destroy(GameObject.Find("Canvas/StartButton"));
    }
    public void DrawDeck()
    {

    }
}

