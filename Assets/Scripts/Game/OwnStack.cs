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



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.transform.CompareTag("MyCard") /*&& Core.GC.PlayOrder[Core.GC.currentPlayerIndex] == -1*/)
            {
               GameObject g = hit.transform.gameObject;
                Debug.Log($"Placed card {g.GetComponent<CardObject>().Card.ID}");
                PlaceCard(g);
            }
        }
        

        if (Input.GetKeyDown(KeyCode.K)) ToggleFocus();
        if (Input.GetKeyDown(KeyCode.B)) StartCoroutine(AddCards(1));
    }

    public void ToggleFocus(bool forceShow = false)
    {
        if (forceShow && focused) return;
        else if (!focused || forceShow) focused = true;
        else focused = false;
        GetComponent<Animator>().Play(focused ? "PutForwards" : "PutAway");
    }

    public void UpdateStack()
    {
        for (int i = 0; i < 16; i++)
        {
            if (i < MyDeck.Count)
            {
                gameObject.transform.Find($"Card ({i})").GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{MyDeck[i].ID}");
                gameObject.transform.Find($"Card ({i})").transform.localScale = new Vector3(-1.86f, -2.6959f, 0.0001f);
            }
            else gameObject.transform.Find($"Card ({i})").transform.localScale = Vector3.zero;
        }
    }
    /// <summary>
    /// Puts a card on the stack and removes it from your own deck
    /// </summary>
    public void PlaceCard(CardObject cardobj)
    {
        Card card = cardobj.Card;
        cardobj.dest = "STACK";
        MyDeck.Remove(card);
        photonView.RPC("PlayPutCardAnimation", RpcTarget.Others, PhotonNetwork.NickName, card.ID);
    }
    /// <summary>
    /// Runs the animation when SOMEONE ELSE puts a card on the stack
    /// </summary>
    /// <param name="user">User that places the card</param>
    /// <param name="cardID">Card that gets placed</param>
    public void PlayPutCardAnimation(string user, string cardID)
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("OtherCards"))
        {
            Player p = g.GetComponent<Player>();
            if (p.name == user)
            {
                GameObject card = GameObject.Find($"{g.name}/Card ({p.cardAmount - 1})");
                card.GetComponent<CardObject>().dest = $"STACK";
                card.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{cardID}");
                p.cardAmount--;
            }
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
        GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("OtherCards"))
        {
            Player p = g.GetComponent<Player>();
            if (p.name == user)
            {
                card.GetComponent<CardObject>().dest = $"{p.destination}/Card ({p.cardAmount})";
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
        delay += UnityEngine.Random.Range(-0.10f, 0.05f);
        for (int i = 0; i < amount; i++)
        {
            AddCard();
            yield return new WaitForSeconds(delay);
        }
    }
}