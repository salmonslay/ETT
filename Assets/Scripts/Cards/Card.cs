using UnityEngine;

public class Card : ScriptableObject
{
    public CardProperties.Color Color;
    public CardProperties.Type Type;
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
        CardTexture = Resources.Load<Texture>(ID);
        obj
    }
}