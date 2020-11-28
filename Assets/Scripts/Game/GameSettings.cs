using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettings : MonoBehaviourPun
{
    public static bool placeMultipleCards = true;
    public Toggle t_placeMultipleCards;

    private void Start()
    {
            t_placeMultipleCards.interactable = PhotonNetwork.IsMasterClient;
    }

    private void Update()
    {
        
    }

    public void SendMultiPlacement(bool allow)
    {
        photonView.RPC("ToggleMultiPlacement", RpcTarget.All, allow);
    }

    [PunRPC]
    private void ToggleMultiPlacement(bool allow)
    {
        placeMultipleCards = allow;
        t_placeMultipleCards.SetIsOnWithoutNotify(allow);
    }
}