using UnityEngine;

public class Card : ScriptableObject
{
    public CardProperties.Color Color;
    public CardProperties.Type Type;
    public int perGame;
    private Texture CardTexture;
    public int number;
    public string ID;
    public GameObject Object;

    public void InitCard(GameObject obj)
    {
        Object = obj;
        if (Type == CardProperties.Type.Number)
            ID = $"{Color}_{number}";
        else
            ID = $"{Color}_{Type}";
        CardTexture = Resources.Load<Texture>($"Cards/{ID}");
        Object.transform.Find("Front").GetComponent<Renderer>().material.mainTexture = CardTexture;
        Object.name = ID;
    }
}