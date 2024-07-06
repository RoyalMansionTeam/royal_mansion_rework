using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public partial class SaveLoadService : ISaveLoadService
    {
        private const string PROGRESS_KEY = "PlayerProgress";

        private readonly IPersistentProgressService _progressService;
        private readonly IMansionFactory _mansionFactory;

        [Inject]
        public SaveLoadService(IPersistentProgressService progressService, IMansionFactory mansionFactory)
        {
            _progressService = progressService;
            _mansionFactory = mansionFactory;
        }

        public void SaveProgress()
        {
            foreach (ISaveWriter progressWriter in _mansionFactory.ProgressWriters)
                progressWriter.SaveProgress(_progressService.Progress);
            PlayerPrefs.SetString(PROGRESS_KEY, _progressService.Progress.ToJson());
        }

        public GameProgress LoadProgress()
        {
            return PlayerPrefs.GetString(PROGRESS_KEY, null)?.ToDeserialized<GameProgress>();
        }

        public void ApplyProgress()
        {
            foreach (ISaveReader progressReader in _mansionFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }
    }
}