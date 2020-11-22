using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DiscordJoinEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordSpectateEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordJoinRequestEvent : UnityEngine.Events.UnityEvent<DiscordRpc.DiscordUser> { }

public class DiscordController : MonoBehaviour
{
    public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
    public string applicationId;
    public string optionalSteamId;
    public DiscordRpc.DiscordUser joinRequest;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;
    public UnityEngine.Events.UnityEvent hasResponded;
    public DiscordJoinEvent onJoin;
    public DiscordJoinEvent onSpectate;
    public DiscordJoinRequestEvent onJoinRequest;

    private DiscordRpc.EventHandlers handlers;

    public static DiscordController Instance;

    public void RequestRespondYes()
    {
        Debug.Log("RPC: responding yes to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Yes);
        hasResponded.Invoke();
    }

    public void RequestRespondNo()
    {
        Debug.Log("RPC: responding no to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
        hasResponded.Invoke();
    }

    public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
    {
        Debug.Log(string.Format("RPC: connected to {0} {1}", connectedUser.userId, connectedUser.avatar));
        Core.avatarLink = $"https://cdn.discordapp.com/avatars/{connectedUser.userId}/{connectedUser.avatar}.png?size=256";
        onConnect.Invoke();

    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        Debug.Log(string.Format("RPC: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(string.Format("RPC: error {0}: {1}", errorCode, message));
    }

    public void JoinCallback(string secret)
    {
        Debug.Log(string.Format("RPC: join ({0})", secret));
        onJoin.Invoke(secret);
    }

    public void SpectateCallback(string secret)
    {
        Debug.Log(string.Format("RPC: spectate ({0})", secret));
        onSpectate.Invoke(secret);
    }

    public void RequestCallback(ref DiscordRpc.DiscordUser request)
    {
        Debug.Log(string.Format("RPC: join request {0}#{1}: {2}", request.username, request.discriminator, request.userId));
        joinRequest = request;
        onJoinRequest.Invoke(request);
    }

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        presence.largeImageKey = "logo";
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        presence.startTimestamp = (long)t.TotalSeconds;
        DiscordRpc.UpdatePresence(presence);
        DiscordRpc.RunCallbacks();
    }

    private void Update()
    {
        if(!Core.avatarLink.Contains("https")) DiscordRpc.RunCallbacks();
    }

    private void OnEnable()
    {
        Debug.Log("RPC: init");
        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback += ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        handlers.joinCallback += JoinCallback;
        handlers.spectateCallback += SpectateCallback;
        handlers.requestCallback += RequestCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    private void OnDisable()
    {
        Debug.Log("RPC: shutdown");
        DiscordRpc.Shutdown();
    }

    private void OnDestroy()
    {
    }
}