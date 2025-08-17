using Photon.Pun;
using Photon.Realtime;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance => instance;
    private static NetworkManager instance;

    private bool isConnected = false;
    private bool isHost => PhotonNetwork.IsMasterClient;
    private string roomId => PhotonNetwork.CurrentRoom.Name;
    private int playerCount => PhotonNetwork.CurrentRoom.PlayerCount;

    public event Action<string, bool, int> OnJoinRoomSuccessfully;
    public event Action OnJoinRoomFail;

    public event Action<bool, int> OnEnteredRoom;
    public event Action<bool, int> OnLeftedRoom;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.ConnectUsingSettings();
    }

    #region Connect To Photon
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        isConnected = true;
    }
    #endregion

    #region Create Room
    public void CreateRoom()
    {
        if (!isConnected) return;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = CONSTANT.MAX_PLAYER };
        string roomId = UnityEngine.Random.Range(CONSTANT.MIN_ROOM_ID, CONSTANT.MAX_ROOM_ID).ToString();
        PhotonNetwork.CreateRoom(roomId, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        CreateRoom();
    }
    #endregion

    #region Join Room
    public void JoinRoom(string roomId) => PhotonNetwork.JoinRoom(roomId);

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        OnJoinRoomSuccessfully?.Invoke(roomId, isHost, playerCount);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        OnJoinRoomFail?.Invoke();
    }
    #endregion

    #region Left Room
    public void LeftRoom() => PhotonNetwork.LeaveRoom();
    #endregion

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        OnEnteredRoom?.Invoke(isHost, playerCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        OnLeftedRoom?.Invoke(isHost, playerCount);
    }
}
