using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    public GameObject B_Name;
    public GameObject B_Join;
    public GameObject B_Create;
    public GameObject I_Name;
    public GameObject I_Room;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            gameObject.SetActive(false);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        GameObject.Find("Canvas/name").GetComponent<InputField>().text = PlayerPrefs.GetString("username", "Player");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        B_Join.SetActive(true);
        B_Create.SetActive(true);
        I_Room.SetActive(true);
        B_Name.SetActive(false);
        I_Name.SetActive(false);
        
    }
    public void Connect()
    {
        PhotonNetwork.NickName = GameObject.Find("Canvas/name").GetComponent<InputField>().text;
        PlayerPrefs.SetString("username", PhotonNetwork.NickName);
        PhotonNetwork.ConnectUsingSettings();
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(GameObject.Find("Canvas/room").GetComponent<InputField>().text);
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(GameObject.Find("Canvas/room").GetComponent<InputField>().text);
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }
}