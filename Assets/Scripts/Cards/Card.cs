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
        //CardTexture = Resources.Load<Texture>($"Cards/{ID}");
        //Object.GetComponent<Renderer>().material.mainTexture = CardTexture;
    }

    /// <summary>
    /// Checks if a card can be put on another
    /// </summary>
    public static bool IsMatch(Card inStack, Card queued)
    {
        //Dropped is a wild card
        if (queued.Color == CardProperties.Color.Wild)
            return true;
        //Colors match
        if (inStack.Color == queued.Color)
            return true;
        //Numbers match
        if (inStack.Type == CardProperties.Type.Number && queued.number == inStack.number)
            return true;
        //Types match (and is not number)
        if (inStack.Type != CardProperties.Type.Number && inStack.Color != CardProperties.Color.Wild && inStack.Type == queued.Type)
            return true;
        if (PlayBoard.currentColor == queued.Color)
            return true;

        return false;
    }
    /// <summary>
    /// Checks if 
    /// </summary>
    /// <param name="inStack"></param>
    /// <param name="queued"></param>
    /// <returns></returns>
    public static bool IsSecondMatch(Card inStack, Card queued)
    {
        //Number match
        if (inStack.Type == CardProperties.Type.Number && queued.Type == CardProperties.Type.Number && inStack.number == queued.number)
            return true;
        //Types match (and is not number)
        if (inStack.Type != CardProperties.Type.Number && inStack.Color != CardProperties.Color.Wild && inStack.Type == queued.Type)
            return true;

        return false;
    }
}