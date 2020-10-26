using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

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
        if(Application.isEditor) GameObject.Find("Canvas/create room").GetComponent<InputField>().text = "dev";
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
    }
    public void Connect()
    {
        PhotonNetwork.NickName = GameObject.Find("Canvas/name").GetComponent<InputField>().text;
        PlayerPrefs.SetString("username", PhotonNetwork.NickName);
        PhotonNetwork.ConnectUsingSettings();
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(GameObject.Find("Canvas/join room").GetComponent<InputField>().text);
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(GameObject.Find("Canvas/create room").GetComponent<InputField>().text);
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}