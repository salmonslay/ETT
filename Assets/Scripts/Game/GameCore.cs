using Photon.Pun;
using UnityEngine;

public class GameCore : MonoBehaviourPun 
{ 
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
}