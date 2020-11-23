using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Core : MonoBehaviourPun
{
    public static string avatarLink = "";

    public static List<string> avatarDB = new List<string>()
    {
        "https://i.imgur.com/o0P47Fa.png", //mio honda
        "https://i.imgur.com/X2BJoSu.png", //kanna kamui eating
        "https://i.imgur.com/qg2qhUh.png", //kobayashi
        "https://i.imgur.com/FrSap3M.png", //hanabi yasuraoka
        "https://i.imgur.com/JqydBzp.png", //ayame kajou
        "https://i.imgur.com/PjfcigK.png", //hyouka fuwa
        "https://i.imgur.com/huC6hbo.png", //mitsuri kanroji
        "https://i.imgur.com/tGnn5wy.png", //aqua crab
        "https://i.imgur.com/rg35GNa.png", //zero two
        "https://i.imgur.com/uuQRoCK.png", //yuu koito
        "https://i.imgur.com/l5sA07u.png", //kyu sugardust
        "https://i.imgur.com/Kxmlq5a.png", //himeko momokino
        "https://i.imgur.com/3zJBN9W.png", //vera (caramelldansen)
        "https://i.imgur.com/5YwJjUE.png", //nishikata
        "https://i.imgur.com/AEmrhrn.png", //colonel sanders
        "https://i.imgur.com/yM2FATg.png", //maki nishikino
        "https://i.imgur.com/FhUxum5.png", //miko iino
        "https://i.imgur.com/lvNahgR.png", //umaru doma
        "https://i.imgur.com/gKVW2TF.png", //yui hirasawa (k-on)
        "https://i.imgur.com/fjXukDE.png", //wiz
        "https://i.imgur.com/hd4kHeY.png", //rio futaba
        "https://i.imgur.com/wnrrAsX.png", //darkness
        "https://i.imgur.com/dq2Iiq7.png", //yuzu aihara
        "https://i.imgur.com/47kAev5.png", //mei aihara
        "https://i.imgur.com/M06m0hV.png", //naomi misora
        "https://i.imgur.com/pC3w5hl.png", //near
        "https://i.imgur.com/IRMxXT3.png", //rem (death note)
        "https://i.imgur.com/IBsGBmy.png", //hana uzaki
        "https://i.imgur.com/XsdrsJZ.png", //akihiko asai
        "https://i.imgur.com/fnFa2fY.png", //kazuya kinoshita
        "https://i.imgur.com/SvrnIe8.png", //malty melromarc
        "https://i.imgur.com/9pUWSQP.png", //legosi
        "https://i.imgur.com/nk0puR7.png", //sayori
        "https://i.imgur.com/UgZcxLz.png", //astolfo
        "https://i.imgur.com/BrbUFE5.png", //hibiki sakura
        "https://i.imgur.com/uiXhtc9.png", //naruzou machio normal
        "https://i.imgur.com/GbkEnBi.png", //mai sakurajima bunny
        "https://i.imgur.com/jzfT7Mh.png", //emilia
        "https://i.imgur.com/hxl4fXH.png", //nagatoro
        "https://i.imgur.com/EBaXShH.png", //rintarou okabe
        "https://i.imgur.com/ij1jm9r.png", //violet evergarden
        "https://i.imgur.com/t0VpL94.png", //shouko nishimiya
        "https://i.imgur.com/X88q5UN.png", //koyomi araragi
        "https://i.imgur.com/vtGaqUF.png", //light yagami
        "https://i.imgur.com/VkpkbqT.png", //itaru hashida
        "https://i.imgur.com/IQnYaet.png", //comfy asakusa
        "https://i.imgur.com/cNxJa9F.png", //monika
        "https://i.imgur.com/3zA1K6l.png", //mina hibino
        "https://i.imgur.com/AeGNeA0.png", //mami nanami
        "https://i.imgur.com/EEFTGaA.png", //mai sakurajima
        "https://i.imgur.com/lneDF0W.png", //mio honda
        "https://i.imgur.com/BOMaSCm.png", //L
        "https://i.imgur.com/gnUqYC1.png", //aqua
        "https://i.imgur.com/29necaZ.png", //ruka sarashina
        "https://i.imgur.com/VEdYKXM.png", //kanna kamui
        "https://i.imgur.com/5TELiih.png", //yuu ishigami
        "https://i.imgur.com/6k3l3wI.png", //sakuta azusagawa
        "https://i.imgur.com/d5aNsWY.png", //satou kazuma
        "https://i.imgur.com/C7Wht7q.png", //saki kamisato
        "https://i.imgur.com/nLWkwy8.png", //hideri kanzeki
        "https://i.imgur.com/vU3GKLM.png", //naruzou machio
        "https://i.imgur.com/4mE3NNL.png", //ryuk
        "https://i.imgur.com/7E5rDOG.png" //sakamoto
    };

    public int avatarID = 0;
    public Card[] Cards; //One of each
    public Card[] FullDeck = new Card[108]; //Contains exactly as many cards as it should
    public OwnStack Stack;
    public List<string> PlayerList = new List<string>();
    public List<string> AvatarList = new List<string>();
    public int playerCount;
    public GameCore GC;
    public Button buttonSkip;
    public Button buttonEtt;
    public Text textEtt;

    public bool started = false;

    [HideInInspector]
    public Download dl;

    /// <summary>
    /// Starts pre-loading the game and cards.
    /// </summary>
    private void Awake()
    {
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
        if (SceneManager.GetActiveScene().name != "game") return;

        GC = GetComponent<GameCore>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "game") return;
        dl = Download.Init();

        //set profile picture
        System.Random rnd = new System.Random();
        avatarDB = avatarDB.OrderBy(x => rnd.Next()).ToList();
        if (avatarLink.Contains("https")) avatarDB.Add(avatarLink);
        avatarID = UnityEngine.Random.Range(0, avatarDB.Count);
        if (!avatarLink.Contains("https")) avatarLink = avatarDB[avatarID];
        photonView.RPC("AddPlayer", RpcTarget.MasterClient, PhotonNetwork.NickName, avatarLink);

        GameObject.Find("StartWorldCanvas/RoomName").GetComponent<Text>().text = $"Room: {PhotonNetwork.CurrentRoom.Name}";

        //Hide template texts and cards
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("TemplateCard")) c.transform.localScale = new Vector3(c.transform.localScale.x, 0, 0.00001f);
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("PlayerName")) t.GetComponent<Text>().text = "";
    }

    IEnumerator PlayStartAnimation()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 16; i++)
        {
            GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
            Destroy(card.GetComponent<CardObject>());
            card.transform.position = new Vector3(-15.21f + UnityEngine.Random.Range(-0.5f, 0.5f), 50 + i * 3, -13.96f + UnityEngine.Random.Range(-0.5f, 0.5f));
            card.AddComponent<Rigidbody>();
            card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            Destroy(card.GetComponent<Rigidbody>(), 13);
        }
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
    private void ConfigureGame(string topCardID, string masterName, bool allowMultiPlace)
    {
        GC.SortPlayerList();
        StartCoroutine(Stack.AddCards(7));
        Destroy(GameObject.Find("Canvas/StartButton"));
        Destroy(GameObject.Find("Canvas/AvatarChange"));
        Destroy(GameObject.Find("Canvas/AllowMultipleCards"));
        Settings.placeMultipleCards = allowMultiPlace;
        //Create top card
        GC.currentTop = CardFromID(topCardID);
        GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
        card.GetComponent<CardObject>().dest = "STACK";
        card.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{GC.currentTop.ID}");

        GC.currentPlayerName = masterName;
        GC.currentPlayerIndex = PlayerList.IndexOf(masterName);
        Debug.Log($"Current player is {masterName} at index {GC.currentPlayerIndex} and spot {GC.PlayOrder[GC.currentPlayerIndex]}");

        //play animations
        Camera.main.GetComponent<Animator>().Play("gameGoUp");
        GameObject.Find("Canvas/Players").GetComponent<Animator>().Play("playerlistIn");
        buttonEtt.GetComponent<Animator>().Play("buttonIn");
        buttonSkip.GetComponent<Animator>().Play("buttonIn");
        StartCoroutine(PlayStartAnimation());
        started = true;
    }

    /// <summary>
    /// Runs on master client in lobbies
    /// </summary>
    [PunRPC]
    public void AddPlayer(string name, string avatar)
    {
        PlayerList.Add(name);
        AvatarList.Add(avatar);
        photonView.RPC("DownloadPlayerlist", RpcTarget.All, string.Join("#", PlayerList.ToArray()), string.Join("#", AvatarList.ToArray()));
    }

    /// <summary>
    /// Runs when game starts
    /// </summary>
    [PunRPC]
    public void DownloadPlayerlist(string namelist, string avatarlist)
    {
        string[] Players = namelist.Split('#');
        string[] Avatars = avatarlist.Split('#');
        GameObject.Find("Canvas/Players").GetComponent<Text>().text = $"Room: <b>{PhotonNetwork.CurrentRoom.Name}</b>\nPlayers ({Players.Length}):\n- <b>{string.Join("</b>\n- <b>", Players)}</b>";

        Stack.myID = Array.IndexOf(Players, PhotonNetwork.NickName);
        playerCount = Players.Length;
        PlayerList = Players.ToList();
        AvatarList.Clear();
        AvatarList = Avatars.ToList();
        GameObject.Find("Canvas/StartButton").GetComponent<Button>().interactable = PhotonNetwork.IsMasterClient && playerCount > 1;
        GameObject.Find("Canvas/DEBUG").GetComponent<Text>().text = string.Join("\n", AvatarList);
        GameObject.Find("Canvas/DEBUG").GetComponent<Text>().text += "\n" + string.Join("\n", PlayerList);
        for (int i = 0; i < PlayerList.Count; i++)
        {
            GameObject.Find($"StartWorldCanvas/User ({i})/Text").GetComponent<Text>().text = Players[i];
            if (Avatars[i].Contains("https://"))
                dl.ChangeAlpha(GameObject.Find($"StartWorldCanvas/User ({i})/Avatar").GetComponent<RawImage>(), Avatars[i]);
        }
    }

    public void ChangeAvatar(int modifier)
    {
        avatarID += modifier;
        if (avatarID == avatarDB.Count) avatarID = 0;
        if (avatarID == -1) avatarID = avatarDB.Count - 1;
        avatarLink = avatarDB[avatarID];
        photonView.RPC("EditAvatarCall", RpcTarget.All, PhotonNetwork.NickName, avatarLink);
    }

    [PunRPC]
    private void EditAvatarCall(string name, string avatar)
    {
        AvatarList[PlayerList.IndexOf(name)] = avatar;
        dl.ChangeAlpha(GameObject.Find($"StartWorldCanvas/User ({PlayerList.IndexOf(name)})/Avatar").GetComponent<RawImage>(), avatar);
    }

    #region Master

    /// <summary>
    /// Starts the game for everyone
    /// </summary>
    public void StartButton()
    {
        //Picks a card that is a number
        Card first = FullDeck[UnityEngine.Random.Range(0, 108)];
        while (first.Type != CardProperties.Type.Number) first = FullDeck[UnityEngine.Random.Range(0, 108)];
        photonView.RPC("DownloadPlayerlist", RpcTarget.All, string.Join("#", PlayerList.ToArray()), string.Join("#", AvatarList.ToArray()));
        photonView.RPC("ConfigureGame", RpcTarget.All, first.ID, PhotonNetwork.NickName, Settings.placeMultipleCards);
    }

    #endregion Master
}