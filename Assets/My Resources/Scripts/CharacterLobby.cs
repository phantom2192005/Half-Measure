using UnityEngine;

public class CharacterLobby : MonoBehaviour
{
    [SerializeField] GameObject hostCharacterPrefab;
    [SerializeField] GameObject guestCharacterPrefab;

    private void OnEnable()
    {
        NetworkManager.Instance.OnJoinRoomSuccessfully += OnJoinRoom;
        NetworkManager.Instance.OnEnteredRoom += OnEnteredRoom;
        NetworkManager.Instance.OnLeftedRoom += OnLeftedRoom;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.OnJoinRoomSuccessfully -= OnJoinRoom;
        NetworkManager.Instance.OnEnteredRoom -= OnEnteredRoom;
        NetworkManager.Instance.OnLeftedRoom -= OnLeftedRoom;
    }

    private void OnJoinRoom(string roomId, bool isHost, int playerCount) => UpdateCharacterLobby(playerCount);

    private void OnEnteredRoom(bool isHost, int playerCount) => UpdateCharacterLobby(playerCount);

    private void OnLeftedRoom(bool isHost, int playerCount) => UpdateCharacterLobby(playerCount);

    private void UpdateCharacterLobby(int playerCount)
    {
        hostCharacterPrefab.SetActive(true);
        guestCharacterPrefab.SetActive(playerCount > 1);
    }
}
