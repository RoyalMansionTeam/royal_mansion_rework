using RoyalMasion.Code.Extensions;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public partial class SaveLoadService : ISaveLoadService
    {
        private const string PROGRESS_KEY = "PlayerProgress";

        private readonly IPersistentProgressService _progressService;

        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SaveProgress()
        {
            PlayerPrefs.SetString(PROGRESS_KEY, _progressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress()
        {
            return PlayerPrefs.GetString(PROGRESS_KEY, null)?.ToDeserialized<PlayerProgress>();
        }
    }
}