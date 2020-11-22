using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviourPun
{
    public static bool placeMultipleCards = true;
    public Toggle t_placeMultipleCards;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "game")
            t_placeMultipleCards.interactable = PhotonNetwork.IsMasterClient;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) AudioListener.volume = AudioListener.volume == 0.3f ? 0 : 0.3f;
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