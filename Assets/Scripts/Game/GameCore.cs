using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoBehaviourPun
{
    private Core Core;
    public string currentPlayerName;
    public int currentPlayerIndex;
    public bool isReverse = false;
    public readonly int[][] PlayOrder = new int[][]
    {
        new int[]{ -1, 0 }, //2p
        new int[]{ -1, 1, 0} , //3p
        new int[]{ -1, 1, 0, 2 }, //4p
        new int[]{ -1, 1, 0, 3, 2 }, //5p
        new int[]{ -1, 1, 4, 0, 3, 2 }, //6p
        new int[]{ -1, 1, 4, 5, 0, 3, 2 } //7p
    };

    

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
        GameObject.Find("Canvas/DEBUG").GetComponent<Text>().text = string.Join(", ", Core.PlayerList);
        for (int i = 1; i < Core.PlayerList.Count; i++)
        {
            GameObject.Find($"OtherCards ({PlayOrder[Core.PlayerList.Count - 1][i]})/Canvas/Text").GetComponent<Text>().text = $"{Core.PlayerList[i]} ({i})";
            GameObject.Find($"OtherCards ({PlayOrder[Core.PlayerList.Count - 1][i]})").GetComponent<Player>().name = Core.PlayerList[i];
            GameObject.Find($"OtherCards ({PlayOrder[Core.PlayerList.Count - 1][i]})").GetComponent<Player>().destination = $"OtherCards ({PlayOrder[Core.PlayerList.Count - 1][i]})";
        }
    }
    /// <summary>
    /// Variables controlled and method ran by the Game Master
    /// </summary>
    #region Master

    #endregion
}