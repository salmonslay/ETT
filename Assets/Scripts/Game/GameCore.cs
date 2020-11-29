using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoBehaviourPun
{
    private Core Core;

    public readonly int[][] PlayOrders = new int[][]
    {
        new int[]{ -1, 0 }, //2p
        new int[]{ -1, 1, 0} , //3p
        new int[]{ -1, 1, 0, 2 }, //4p
        new int[]{ -1, 1, 0, 3, 2 }, //5p
        new int[]{ -1, 1, 4, 0, 3, 2 }, //6p
        new int[]{ -1, 1, 4, 5, 0, 3, 2 }, //7p
        new int[]{ -1, 1, 4, 5, 0, 6, 3, 2 }, //8p
        new int[]{ -1, 1, 4, 5, 0, 6, 7, 3, 2 } //9p
    };

    //will be one of those above
    public int[] PlayOrder;

    public string currentPlayerName;
    public int currentPlayerIndex;
    public bool isReverse = false;
    public Card currentTop = null;

    private void Start()
    {
        Core = GetComponent<Core>();
    }

    /// <summary>
    /// Makes the play order synced between clients
    /// https://i.imgur.com/hLg8zTn.png
    /// </summary>
    public void SortPlayerList()
    {
        if (Core.playerCount == 1) return;
        List<string> namesBefore = Core.PlayerList.GetRange(0, Core.Stack.myID);
        Core.PlayerList.RemoveRange(0, Core.Stack.myID);
        Core.PlayerList.AddRange(namesBefore);

        List<string> avatarsBefore = Core.AvatarList.GetRange(0, Core.Stack.myID);
        Core.AvatarList.RemoveRange(0, Core.Stack.myID);
        Core.AvatarList.AddRange(avatarsBefore);

        GameObject.Find("Canvas/DEBUG").GetComponent<Text>().text = string.Join(", ", Core.PlayerList);
        PlayOrder = PlayOrders[Core.PlayerList.Count - 2];
        for (int i = 1; i < Core.PlayerList.Count; i++)
        {
            GameObject.Find($"OtherCards ({PlayOrder[i]})/Canvas/Text").GetComponent<Text>().text = $"{Core.PlayerList[i]} ({i})";
            Core.dl.ChangeAlpha(GameObject.Find($"OtherCards ({PlayOrder[i]})/Canvas/GameAvatar").GetComponent<RawImage>(), Core.AvatarList[i]);
            GameObject.Find($"OtherCards ({PlayOrder[i]})").GetComponent<Player>().name = Core.PlayerList[i];
            GameObject.Find($"OtherCards ({PlayOrder[i]})").GetComponent<Player>().destination = $"OtherCards ({PlayOrder[i]})";
        }
    }

    /// <summary>
    /// Gets index of next player
    /// </summary>
    public int NextPlayer()
    {
        int newIndex;

        if (isReverse)
        {
            newIndex = currentPlayerIndex - 1;
        }
        else
        {
            newIndex = currentPlayerIndex + 1;
        }
        if (newIndex == PlayOrder.Length) newIndex = 0;
        if (newIndex == -1) newIndex = PlayOrder.Length - 1;
        return newIndex;
    }
}