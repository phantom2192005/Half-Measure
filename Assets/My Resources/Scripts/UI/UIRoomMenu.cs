using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Half_Measure.UI
{
    public class UIRoomMenu : UIBase
    {
        [SerializeField] Button startButton;
        [SerializeField] Button backButton;
        [SerializeField] Button coppyRoomIdButton;
        [SerializeField] TextMeshProUGUI roomIdTMP;
        [SerializeField] TextMeshProUGUI hostTMP;
        [SerializeField] TextMeshProUGUI guestTMP;

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            backButton.onClick.AddListener(OnBackButtonClicked);
            coppyRoomIdButton.onClick.AddListener(OnCopyRoomIdButtonClicked);
            NetworkManager.Instance.OnEnteredRoom += UpdateRoleUI;
            NetworkManager.Instance.OnLeftedRoom += UpdateRoleUI;
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            backButton.onClick.RemoveListener(OnBackButtonClicked);
            coppyRoomIdButton.onClick.RemoveListener(OnCopyRoomIdButtonClicked);
            NetworkManager.Instance.OnEnteredRoom -= UpdateRoleUI;
            NetworkManager.Instance.OnLeftedRoom -= UpdateRoleUI;
        }

        private void OnStartButtonClicked()
        {
            UIManager.Instance.OpenUI<UILoadingScene>();
            LoadingSceneManager.Instance.LoadScene(SceneType.GameScene);
        }

        private void OnBackButtonClicked()
        {
            NetworkManager.Instance.LeftRoom();
            UIManager.Instance.OpenUI<UIMainMenu>();
        }

        private void OnCopyRoomIdButtonClicked() => GUIUtility.systemCopyBuffer = roomIdTMP.text.Substring(9);

        public void UpdateRoomInfoUI(string roomId, bool isHost, int playerCount)
        {
            roomIdTMP.text = "Room Id: " + roomId;
            UpdateRoleUI(isHost, playerCount);
        }

        private void UpdateRoleUI(bool isHost, int playerCount)
        {
            startButton.gameObject.SetActive(isHost);
            startButton.interactable = playerCount == CONSTANT.MAX_PLAYER;
            if (isHost)
            {
                hostTMP.text = CONSTANT.ME;
                if (playerCount == CONSTANT.MAX_PLAYER)
                {
                    guestTMP.gameObject.SetActive(true);
                    guestTMP.text = CONSTANT.GUEST;
                }
                else guestTMP.gameObject.SetActive(false);
            }
            else
            {
                hostTMP.text = CONSTANT.HOST;
                guestTMP.text = CONSTANT.ME;
            }
        }
    }
}