using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnStack : MonoBehaviourPun
{
    public int myID = -1;
    private bool focused = true;
    public List<Card> MyDeck = new List<Card>();
    public Core Core;

    public bool hasPut = false;
    public bool hasEtt = false;

    public int pickFromStack;

    //focused wild
    private string oldPos;

    private CardObject currentFocus;

    public GameObject WildColorSelection;
    public GameObject WildDrawSelection;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //put card on board
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.transform.CompareTag("MyCard") && oldPos == null && Core.GC.PlayOrder[Core.GC.currentPlayerIndex] == -1)
            {
                CardObject g = hit.transform.gameObject.GetComponent<CardObject>();
                if (hasPut && !Settings.placeMultipleCards) return;
                if (g.Card.Color == CardProperties.Color.Wild)
                {
                    oldPos = g.dest;
                    g.dest = "MyCards/Wild";
                    if (g.Card.Type == CardProperties.Type.Draw) WildDrawSelection.SetActive(true);
                    else WildColorSelection.SetActive(true);
                    currentFocus = g;
                }
                else if (!hasPut && Card.IsMatch(Core.GC.currentTop, g.Card))
                {
                    PutCard(g);
                    hasPut = true;
                }
                else if (hasPut && Card.IsSecondMatch(Core.GC.currentTop, g.Card))
                {
                    PutCard(g);
                }
            }
            //pick up card from stack
            //triggers by click on stack or B
            else if((Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit2) && hit2.transform.CompareTag("CardStack") || Input.GetKeyDown(KeyCode.B)) && Core.GC.PlayOrder[Core.GC.currentPlayerIndex] == -1)
            {
                pickFromStack++;
                StartCoroutine(AddCards(1));
                hasEtt = MyDeck.Count == 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.K)) ToggleFocus();
        if (Input.GetMouseButtonDown(0)) Core.textEtt.gameObject.SetActive(false);
        if (Core.started)
        {
            Core.buttonEtt.interactable = Core.GC.PlayOrder[Core.GC.currentPlayerIndex] == -1;
            Core.buttonSkip.interactable = Core.GC.PlayOrder[Core.GC.currentPlayerIndex] == -1 && hasPut || pickFromStack > 2;
        }
        
    }

    /// <summary>
    /// Called from game when user places color wild card
    /// </summary>
    public void PlaceWildCard(string color)
    {
        WildColorSelection.SetActive(false);
        WildDrawSelection.SetActive(false);
        if (color == "cancel") currentFocus.dest = oldPos;
        else
        {
            CardProperties.Color c = CardProperties.Color.Red;
            if (color == "green") c = CardProperties.Color.Green;
            if (color == "yellow") c = CardProperties.Color.Yellow;
            if (color == "blue") c = CardProperties.Color.Blue;
            PlayBoard.SetColor(c);
            PutCard(currentFocus);
            hasPut = true;
        }
        oldPos = null;
    }

    public void ToggleFocus(bool forceShow = false)
    {
        if (forceShow && focused) return;
        else if (!focused || forceShow) focused = true;
        else focused = false;
        GetComponent<Animator>().Play(focused ? "PutForwards" : "PutAway");
    }

    /// <summary>
    /// Replaces all the cards in the stack to prevent blank spots
    /// </summary>
    public void UpdateStack()
    {
        int i = 0;
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("MyCard"))
        {
            if (card.name != "Back")
            {
                card.GetComponentInParent<CardObject>().dest = $"MyCards/Card ({i})";
                i++;
            }
        }
    }

    /// <summary>
    /// Puts a card on the stack and removes it from your own deck
    /// </summary>
    public void PutCard(CardObject cardobj)
    {
        cardobj.gameObject.tag = "InStack";
        Card card = cardobj.Card;
        cardobj.dest = "STACK";
        cardobj.moveInStack = true;
        Core.GC.currentTop = cardobj.Card;
        MyDeck.Remove(card);
        if (card.Color != CardProperties.Color.Wild) PlayBoard.SetColor(card.Color);
        photonView.RPC("PlayPutCardAnimation", RpcTarget.Others, PhotonNetwork.NickName, card.ID, (int)PlayBoard.currentColor);
        UpdateStack();
        hasEtt = MyDeck.Count == 1;
    }

    /// <summary>
    /// Runs the animation when SOMEONE ELSE puts a card on the stack
    /// </summary>
    /// <param name="user">User that places the card</param>
    /// <param name="cardID">Card that gets placed</param>
    [PunRPC]
    public void PlayPutCardAnimation(string user, string cardID, int color)
    {
        Core.GC.currentTop = Core.CardFromID(cardID);
        if (color != 4) PlayBoard.SetColor((CardProperties.Color)color);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("OtherCards"))
        {
            Player p = g.GetComponent<Player>();
            if (p.name == user)
            {
                GameObject card = GameObject.Find($"{user}#{p.cardAmount - 1}");
                card.name = "dropped";
                card.GetComponent<CardObject>().dest = $"STACK";
                card.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{cardID}");
                p.cardAmount--;
                GameObject.Find(p.destination + "/Canvas/Text").GetComponent<Text>().text = $"{user}\n{p.cardAmount} cards";
            }
        }
    }

    /// <summary>
    /// Runs action when place a card
    /// </summary>
    [PunRPC]
    public void PutCardAction()
    {
        Core.textEtt.gameObject.SetActive(false);
        hasPut = false;
        pickFromStack = 0;
        Card card = Core.GC.currentTop;

        //Take cards if you're next
        if (card.Type == CardProperties.Type.Draw && Core.GC.PlayOrder[Core.GC.NextPlayer()] == -1)
        {
            if (card.Color == CardProperties.Color.Wild) StartCoroutine(AddCards(4));
            else StartCoroutine(AddCards(2));
        }
        else if (card.Type == CardProperties.Type.Reverse)
        {
            Core.GC.isReverse = !Core.GC.isReverse;
        }
        if (card.Type == CardProperties.Type.Skip)
        {
            Core.GC.currentPlayerIndex = Core.GC.NextPlayer();
            Core.GC.currentPlayerIndex = Core.GC.NextPlayer();
        }
        else
        {
            Core.GC.currentPlayerIndex = Core.GC.NextPlayer();
        }
    }

    /// <summary>
    /// Adds one card to your own deck, and asks everyone to play the animation for it
    /// </summary>
    public void AddCard()
    {
        Card randomized = Core.FullDeck[UnityEngine.Random.Range(0, 108)];
        this.photonView.RPC("PlayTakeCardAnimation", RpcTarget.Others, PhotonNetwork.NickName);

        GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
        CardObject cardObject = card.GetComponent<CardObject>();
        cardObject.dest = $"MyCards/Card ({MyDeck.Count})";
        card.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{randomized.ID}");
        card.name = randomized.ID;
        card.tag = "MyCard";
        cardObject.Card = randomized;
        MyDeck.Add(randomized);
    }

    /// <summary>
    /// Runs animation when SOMEONE ELSE takes a card
    /// </summary>
    /// <param name="user">User that takes the card</param>
    [PunRPC]
    public void PlayTakeCardAnimation(string user)
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("OtherCards"))
        {
            Player p = g.GetComponent<Player>();
            if (p.name == user)
            {
                GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
                card.name = $"{user}#{p.cardAmount}";
                card.GetComponent<CardObject>().dest = $"{p.destination}/Card ({(p.cardAmount > 14 ? 14 : p.cardAmount)})"; //place at spot 14 if deck full
                p.cardAmount++;
                GameObject.Find(p.destination + "/Canvas/Text").GetComponent<Text>().text = $"{user}\n{p.cardAmount} cards";
            }
        }
    }

    /// <summary>
    /// Adds multiple cards with a slight delay
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator AddCards(int amount, float delay = 0.15f)
    {
        delay += UnityEngine.Random.Range(-0.05f, 0.05f);
        for (int i = 0; i < amount; i++)
        {
            AddCard();
            yield return new WaitForSeconds(delay);
        }
    }

    public void ETT()
    {
        if (hasEtt)
        {
            hasEtt = false;
            photonView.RPC("AnnounceETT", RpcTarget.All, true, PhotonNetwork.NickName);
        }
        else
        {
            photonView.RPC("AnnounceETT", RpcTarget.All, false, PhotonNetwork.NickName);
            StartCoroutine(AddCards(2));
            //play effect
        }
    }

    [PunRPC]
    public void AnnounceETT(bool success, string name)
    {
        Core.textEtt.gameObject.SetActive(true);
        if (success)
        {
            Core.textEtt.color = new Color(0.1168802f, 0.9339623f, 0.08370414f);
            Core.textEtt.text = $"{name} has Ett!";
        }
        else
        {
            Core.textEtt.color = Color.red;
            Core.textEtt.text = $"{name} tried to claim Ett!";
        }
    }

    public void TryToContinue()
    {
        if (hasEtt)
        {
            StartCoroutine(AddCards(2));
            hasEtt = false;
            Core.textEtt.text = $"{name} forgot to claim Ett!";
        }
        else if (hasPut || pickFromStack > 2)
        {
            photonView.RPC("PutCardAction", RpcTarget.All);
        }
    }
}