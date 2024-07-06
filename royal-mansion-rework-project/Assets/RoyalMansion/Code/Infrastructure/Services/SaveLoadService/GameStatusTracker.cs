using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public class GameStatusTracker : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                return;
            SaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
                return;
            SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void SaveData()
        {
            _saveLoadService.SaveProgress();
        }

    }
}