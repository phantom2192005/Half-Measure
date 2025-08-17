using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Half_Measure 
{
    [RequireComponent(typeof(PhotonView))]
    public class LoadingSceneManager : Singleton<LoadingSceneManager>
    {
        private PhotonView photonView;
        private int sceneLoadedCount = 0;
        public event Action<SceneType> OnSceneLoaded;

        protected override void LoadComponents()
        {
            photonView = photonView != null ? photonView : GetComponent<PhotonView>();
        }

        public void LoadScene(SceneType sceneType) => photonView.RPC("RPC_WaitLoadScene", RpcTarget.All, sceneType);

        [PunRPC]
        private void RPC_WaitLoadScene(SceneType sceneType)
        {
            StartCoroutine(WaitLoadScene(sceneType));
        }

        private IEnumerator WaitLoadScene(SceneType sceneType)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneType);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f) yield return null;
            photonView.RPC("RPC_SceneLoaded", RpcTarget.All);
            while (sceneLoadedCount < CONSTANT.MAX_PLAYER) yield return null;
            asyncOperation.allowSceneActivation = true;
            OnSceneLoaded?.Invoke(sceneType);
        }

        [PunRPC]
        private void RPC_SceneLoaded() => sceneLoadedCount++;
    }

    public enum SceneType
    {
        MainScene,
        GameScene
    }
}

