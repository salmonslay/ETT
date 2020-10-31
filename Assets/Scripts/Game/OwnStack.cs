﻿using Photon.Pun;
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.transform.CompareTag("MyCard") && Core.GC.currentPlayerName == PhotonNetwork.NickName)
            {
                Debug.Log(hit.transform.gameObject.name);
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

    public void AddCard()
    {
        Card randomized = Core.FullDeck[UnityEngine.Random.Range(0, 108)];
        //PlayCardAnimation(randomized.ID, PhotonNetwork.NickName);
        this.photonView.RPC("PlayCardAnimation", RpcTarget.Others, randomized.ID, PhotonNetwork.NickName);

        GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
        CardObject cardObject = card.GetComponent<CardObject>();
        cardObject.dest = $"MyCards/Card ({MyDeck.Count})";
        card.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{randomized.ID}");
        card.name = randomized.ID;
        cardObject.Card = randomized;
        MyDeck.Add(randomized);
    }

    [PunRPC]
    public void PlayCardAnimation(string cardID, string user)
    {
        GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("OtherCards"))
        {
            Player p = g.GetComponent<Player>();
            if (p.name == user)
            {
                card.GetComponent<CardObject>().dest = p.destination + $"/Card ({p.cardAmount})";
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
        delay += UnityEngine.Random.Range(-0.1f, 0.05f);
        for (int i = 0; i < amount; i++)
        {
            AddCard();
            yield return new WaitForSeconds(delay);
        }
    }
}