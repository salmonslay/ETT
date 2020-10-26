using UnityEngine;

public class Card : ScriptableObject
{
    public CardProperties.Color Color;
    public CardProperties.Type Type;
    public int perGame;
    private Texture CardTexture;
    public int number;
    public string ID;
    public bool isDropped;

    public void InitCard()
    {
        if (Type == CardProperties.Type.Number)
            ID = $"{Color}_{number}";
        else
            ID = $"{Color}_{Type}";
        CardTexture = Resources.Load<Texture>($"Cards/{ID}");
        //Object.GetComponent<Renderer>().material.mainTexture = CardTexture;
    }
}