using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoBehaviourPun 
{
    private Core Core;
    public readonly int[][] PlayOrder = new int[][]
    {
        new int[]{0, 1 },
        new int[]{0, 2, 1} ,
        new int[]{0, 2, 1, 3 },
        new int[]{0, 2, 1, 4, 3 },
        new int[]{0, 2, 5, 1, 4, 3 },
        new int[]{0, 2, 5, 6, 1, 4, 3 },
        new int[]{0, 2, 5, 6, 1, 7, 4, 3 }
    };

    public bool isReverse = false;
    private void Start()
    {
        Core = GetComponent<Core>();
    }
    public void PlacePlayers()
    {
        int players = Core.playerCount;
        for (int i = Core.Stack.myID+1; i < players + Core.Stack.myID; i++)
        {
            if (i != Core.Stack.myID)
            {
                int slot = PlayOrder[players][i];
                GameObject.Find($"OtherCards ({slot})/Canvas/Text").GetComponent<Text>().text = $"{Core.PlayerList[i]} ({i})";
                GameObject.Find($"OtherCards ({slot})").GetComponent<Player>().name = Core.PlayerList[i];
                GameObject.Find($"OtherCards ({slot})").GetComponent<Player>().id = i;
            }
        }
    }
}