using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Half_Measure.UI
{
    public class UIJoinMenu : UIBase
    {
        [SerializeField] TMP_InputField roomIdInputField;
        [SerializeField] Button joinButton;
        [SerializeField] Button backButton;

        private void OnEnable()
        {
            roomIdInputField.text = "";
            joinButton.onClick.AddListener(OnJoinButtonClicked);
            backButton.onClick.AddListener(OnBackButtonClicked);
            NetworkManager.Instance.OnJoinRoomSuccessfully += OnJoinRoomSuccessfully;
            NetworkManager.Instance.OnJoinRoomFail += OnJoinRoomFailed;
        }

        private void OnDisable()
        {
            joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            backButton.onClick.RemoveListener(OnBackButtonClicked);
            NetworkManager.Instance.OnJoinRoomFail -= OnJoinRoomFailed;
        }

        private void OnBackButtonClicked() => UIManager.Instance.BackUI();

        private void OnJoinButtonClicked() => NetworkManager.Instance.JoinRoom(roomIdInputField.text);

        private void OnJoinRoomSuccessfully(string roomId, bool isHost, int playerCount) => UIManager.Instance.OpenUI<UIRoomMenu>().UpdateRoomInfoUI(roomId, isHost, playerCount);

        private void OnJoinRoomFailed()
        {
            roomIdInputField.text = "";
            //ToastManager.Instance.Notify("Room Id Not Correct !");
        }
    }
}