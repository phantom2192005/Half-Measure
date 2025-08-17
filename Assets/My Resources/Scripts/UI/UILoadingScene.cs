namespace Half_Measure.UI 
{
    public class UILoadingScene : UIBase
    {
        private void OnEnable()
        {
            LoadingSceneManager.Instance.OnSceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            LoadingSceneManager.Instance.OnSceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(SceneType sceneType)
        {
            if (sceneType == SceneType.MainScene) UIManager.Instance.OpenUI<UIMainMenu>();
            else if (sceneType == SceneType.GameScene) UIManager.Instance.OpenUI<UIGameHUD>();
        }
    }
}
