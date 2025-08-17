using UnityEngine;
using UnityEngine.UI;

namespace Half_Measure.UI 
{
    public class UIMainMenu : UIBase
    {
        [SerializeField] Button playButton;
        [SerializeField] Button joinButton;
        [SerializeField] Button settingButton;
        [SerializeField] Button quitButton;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            joinButton.onClick.AddListener(OnJoinButtonClicked);
            settingButton.onClick.AddListener(OnSettingButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            NetworkManager.Instance.OnJoinRoomSuccessfully += OnJoinRoomSuccessfully;
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
            joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            settingButton.onClick.RemoveListener(OnSettingButtonClicked);
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
            NetworkManager.Instance.OnJoinRoomSuccessfully -= OnJoinRoomSuccessfully;
        }

        private void OnPlayButtonClicked() => NetworkManager.Instance.CreateRoom();

        private void OnJoinRoomSuccessfully(string roomId, bool isHost, int playerCount) => UIManager.Instance.OpenUI<UIRoomMenu>().UpdateRoomInfoUI(roomId, isHost, playerCount);

        private void OnJoinButtonClicked() => UIManager.Instance.OpenUI<UIJoinMenu>();

        private void OnSettingButtonClicked() => UIManager.Instance.OpenUI<UISettingMenu>();

        private void OnQuitButtonClicked() => Application.Quit();
    }
}