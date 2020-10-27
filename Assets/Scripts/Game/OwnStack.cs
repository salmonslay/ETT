using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnStack : MonoBehaviour
{
    public int myID = -1;
    bool focused = true;
    public List<Card> MyDeck = new List<Card>();
    public Core CoreInstance;


    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.transform.tag == "MyCard")
        {
            Debug.Log(hit.transform.gameObject.name);
        }

        if (Input.GetKeyDown(KeyCode.K)) ToggleFocus();
        if (Input.GetKeyDown(KeyCode.B)) StartCoroutine(AddCards(1));
    }
    public void ToggleFocus(bool forceShow = false)
    {
        if (!focused || forceShow) focused = true;
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
    private Card AddCard()
    {
        Card randomized = CoreInstance.FullDeck[UnityEngine.Random.Range(0, 108)];
        GameObject card = Instantiate(Resources.Load("Prefabs/TopStack") as GameObject);
        card.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{randomized.ID}");
        card.GetComponent<CardMover>().dest = $"MyCards/Card ({MyDeck.Count})";
        MyDeck.Add(randomized);
        return randomized;
    }
    public IEnumerator AddCards(int amount, float delay = 0.2f)
    {
        for (int i = 0; i < amount; i++)
        {
            AddCard();
            yield return new WaitForSeconds(delay);
        }
    }

}
