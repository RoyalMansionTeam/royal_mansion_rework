using System;
using RoyalMasion.Code.Infrastructure.Data;

namespace RoyalMasion.Code.Infrastructure.Services.SceneLoaderService
{
    public interface ISceneLoader
    {
        void LoadScene(int sceneIndex, Action onLoaded = null);
        void LoadScene(SceneID sceneID, Action onLoaded = null);
    }
}