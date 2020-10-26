using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnStack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.transform.tag == "MyCard")
        {
            Debug.Log(hit.transform.gameObject.name);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

    }
    public void ToggleFocus(bool show)
    {
        GetComponent<Animation>().Play(show ? "PutForwards" : "PutAway");
    }
    public void UpdateStack()
    {
        for (int i = 0; i < 16; i++)
        {
            if (i < Core.Instance.MyDeck.Count)
            {
                gameObject.transform.Find($"Card ({i})").GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Cards/{Core.Instance.MyDeck[i].ID}");
                gameObject.transform.Find($"Card ({i})").transform.localScale = new Vector3(-1.86f, -2.6959f, 0.00959f);
            }
            else gameObject.transform.Find($"Card ({i})").transform.localScale = Vector3.zero;
        }
    }

}
