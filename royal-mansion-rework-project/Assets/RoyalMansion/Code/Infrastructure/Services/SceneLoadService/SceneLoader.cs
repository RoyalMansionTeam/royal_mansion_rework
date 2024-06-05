using System;
using System.Collections;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.DI;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace RoyalMasion.Code.Infrastructure.Services.SceneLoaderService
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        private IEnumerator _loadingRoutine;

        public void LoadScene(int sceneIndex, Action onLoaded = null)
        {
            StartLoadingOperation(sceneIndex, onLoaded);
        }

        public void LoadScene(SceneID sceneID, Action onLoaded = null)
        {
            StartLoadingOperation((int)sceneID, onLoaded);
        }

        private void StartLoadingOperation(int sceneIndex, Action onLoaded)
        {
            Time.timeScale = 1;
            _loadingRoutine = LoadingScreenStartRoutine(sceneIndex, onLoaded);
            StartCoroutine(_loadingRoutine);
        }

        private IEnumerator LoadingScreenStartRoutine(int sceneIndex, Action onLoaded)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (operation != null && !operation.isDone)
                yield return 0;

            _loadingRoutine = null;
            onLoaded?.Invoke();
        }
    }
}