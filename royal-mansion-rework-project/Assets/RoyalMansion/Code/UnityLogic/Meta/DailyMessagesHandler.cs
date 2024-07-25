using RoyalMansion.Code.Extensions;
using RoyalMansion.Code.UnityLogic.DailyRewards;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace RoyalMansion.Code.UnityLogic.Meta
{
    public class DailyMessagesHandler : MonoBehaviour, ISaveWriter, ISaveReader
    {
        public string SaveableID { get; set; }
        public Action<DailyMessageData> AvailableMessageFound;

        [SerializeField] private DailyRewardService _rewardService;

        private IAssetProvider _assetProvider;
        private IMansionFactory _mansionFactory;
        private IStaticDataService _staticData;
        private ProjectTimeTracker _timeTracker;

        private DailyMessagesConfig _messagesConfig;
        private int _nextMessageID;


        [Inject]
        public void Construct(IAssetProvider assetProvider, IMansionFactory mansionFactory,
            IStaticDataService staticData, ProjectTimeTracker timeTracker)
        {
            _assetProvider = assetProvider;
            _mansionFactory = mansionFactory;
            _staticData = staticData;
            _timeTracker = timeTracker;
            RegisterSaveableEntity();
        }

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _timeTracker.ChangedDay += OnDayChanged;
        }

        private void OnDayChanged()
        {
            if (_nextMessageID >= _messagesConfig.DailyMessages.Count)
                return;
            if (_nextMessageID != 0)
                if (!_messagesConfig.DailyMessages[_nextMessageID - 1].SequenceRead)
                    return;
            AvailableMessageFound?.Invoke(_messagesConfig.DailyMessages[_nextMessageID]);
            _nextMessageID++;
        }

        public void LoadProgress(GameProgress progress)
        {
            SetupData(progress);
        }


        public void SaveProgress(GameProgress progress)
        {
            if (_messagesConfig == null)
                return;
            foreach (DailyMessageData messageSequence in _messagesConfig.DailyMessages)
            {
                progress.IngameDataProgress.TryAddMetaMessage(new DailyMessagesSaveData(
                    uniqueSaveID: messageSequence.SequenceID,
                    seuenceRead: messageSequence.SequenceRead,
                    claimedDateTime: messageSequence.ClaimedDateTime
                    ));
            }
        }
        private async void SetupData(GameProgress progress)
        {
            await LoadConfig();
            if (_messagesConfig == null)
                return;
            if (progress == null || progress.IngameDataProgress.DailyMessagesSaveDatas == null)
            {
                TrySetDailyMessage();
                return;
            }
            foreach (DailyMessageData messageSequence in _messagesConfig.DailyMessages)
            {
                foreach (DailyMessagesSaveData savedMessage in progress.IngameDataProgress.DailyMessagesSaveDatas)
                {
                    if (savedMessage.UniqueSaveID != messageSequence.SequenceID)
                        continue;
                    messageSequence.SequenceRead = savedMessage.SequenceRead;
                    messageSequence.ClaimedDateTime = savedMessage.ClaimedDateTime;
                    break;
                }
            }
            TrySetDailyMessage();
        }

        private async Task LoadConfig() =>
            _messagesConfig = await _assetProvider.Load<DailyMessagesConfig>
            (AssetAddress.META_MESSAGES_CONFIG_DATA_PATH);

        private void TrySetDailyMessage()
        {
            DailyMessageData lastMessageSequence = null;
            for(int i = _messagesConfig.DailyMessages.Count-1; i>=0; i--)
            {
                _nextMessageID = i + 1;
                DailyMessageData messageSequence = _messagesConfig.DailyMessages[i];
                if (i == 0 || messageSequence.SequenceRead)
                {
                    AvailableMessageFound?.Invoke(messageSequence);
                    return;
                }
                lastMessageSequence = _messagesConfig.DailyMessages[i - 1];
                if (!messageSequence.SequenceRead & lastMessageSequence.SequenceRead)
                {
                    if (IsTodaysMessage(lastMessageSequence.ClaimedDateTime))
                    {
                        AvailableMessageFound?.Invoke(lastMessageSequence);
                        return;
                    }
                    AvailableMessageFound?.Invoke(messageSequence);
                    return;
                }
            }
        }

        private bool IsTodaysMessage(int claimedDateTime)
        {
            return (Epoch.CurrentRelativeTime(_rewardService.CurrentEpoch.EpochStartDateTime.ToDateTime()) -
                (claimedDateTime + _staticData.GameData.EconomyStaticData.StandartRewardDayDuration.ToFloat())
                >= 0);
        }

        private void RegisterSaveableEntity() =>
            _mansionFactory.RegisterSaveableEntity((ISaveReader)this);

        private void Unsubscribe()
        {
            
        }
        private void OnDestroy()
        {
            Unsubscribe();
        }

    }
}
