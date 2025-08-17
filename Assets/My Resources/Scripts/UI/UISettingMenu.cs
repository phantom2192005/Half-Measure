using UnityEngine;
using UnityEngine.UI;

namespace Half_Measure.UI 
{
    public class UISettingMenu : UIBase
    {
        [SerializeField] Slider sfxSlider;
        [SerializeField] Slider musicSlider;
        [SerializeField] Button backButton;

        public void OnEnable()
        {
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        public void OnDisable()
        {
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnSfxVolumeChanged(float value)
        {
            // Handle SFX volume change
            //AudioManager.Instance.SetSfxVolume(value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            // Handle music volume change
            //AudioManager.Instance.SetMusicVolume(value);
        }

        private void OnBackButtonClicked() => UIManager.Instance.BackUI();
    }
}