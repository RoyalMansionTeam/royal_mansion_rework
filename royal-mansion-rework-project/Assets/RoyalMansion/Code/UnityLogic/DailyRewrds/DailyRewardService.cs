using RoyalMansion.Code.Extensions;
using RoyalMansion.Code.UI.Windows.DailyRewardWindow;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace RoyalMansion.Code.UnityLogic.DailyRewards
{
    public class DailyRewardService : MonoBehaviour, ISaveReader, ISaveWriter
    {
        public string SaveableID { get; set; }
        public Action<bool> FoundUnclaimedReward;

        [SerializeField] private ProjectTimeTrackerComponent _timeTrackerComponent;

        private DailyRewardsStaticData _rewardsDataConfig;
        private RewardEpoch _currentEpoch;
        private IAssetProvider _assetProvider;
        private IMansionFactory _mansionFactory;
        private IStaticDataService _staticData;
        private ProjectTimeTracker _projectTimeTracker;

        private string _claimedRewardID;
        private int _claimedRewardTime;
        private int _nextRewardID;

        public RewardEpoch CurrentEpoch => _currentEpoch;


        [Inject]
        public void Construct(IAssetProvider assetProvider, IMansionFactory mansionFactory, 
            IStaticDataService staticData, ProjectTimeTracker projectTimeTracker)
        {
            _assetProvider = assetProvider;
            _mansionFactory = mansionFactory;
            _staticData = staticData;
            _projectTimeTracker = projectTimeTracker;
            RegisterSaveableEntity();
        }

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _projectTimeTracker.ChangedDay += UpdateRewardsInRealTime;
        }

        private void UpdateRewardsInRealTime()
        {
            if (_nextRewardID>=_currentEpoch.DayData.Count)
            {
                Debug.Log("All rewards claimed");
                return;
            }
            if (_nextRewardID == 0)
                return;
            if (_currentEpoch.DayData[_nextRewardID - 1].State != RewardState.Claimed)
                return;
            _currentEpoch.DayData[_nextRewardID].State = RewardState.Claimable;
            FoundUnclaimedReward?.Invoke(true);
            _nextRewardID++;
        }

        public void LoadProgress(GameProgress progress)
        {
            UpdateRewardData(progress);
            if (progress.IngameDataProgress.LastClaimedRewardData == null)
                return;
            _claimedRewardTime = progress.IngameDataProgress.LastClaimedRewardData.ClaimedDateTime;
            _claimedRewardID = progress.IngameDataProgress.LastClaimedRewardData.UniqueSaveID;
        }

        public void SaveProgress(GameProgress progress)
        {
            progress.IngameDataProgress.WriteClaimedRewardData(new DailyRewardSaveData(
                _claimedRewardID,
                _claimedRewardTime
                ));
        }

        public void UpdateClaimedReward(string ID, int claimedTime)
        {
            _claimedRewardID = ID;
            _claimedRewardTime = claimedTime;
        }

        private async void UpdateRewardData(GameProgress progress)
        {
            if (_rewardsDataConfig == null)
            {
                await LoadConfig();
                if (_rewardsDataConfig == null)
                    return;
            }
            foreach (RewardEpoch rewardEpoch in _rewardsDataConfig.RewardsEpochs)
            {
                _currentEpoch = rewardEpoch;
                if (rewardEpoch.EpochFinished)
                    continue;
                _timeTrackerComponent.SetTimeTrackerEpoch(rewardEpoch.EpochStartDateTime.ToDateTime());
                if (progress.IngameDataProgress.LastClaimedRewardData == null ||
                    progress.IngameDataProgress.LastClaimedRewardData.ClaimedDateTime == 0)
                {
                    rewardEpoch.DayData[0].State = RewardState.Claimable;
                    FoundUnclaimedReward?.Invoke(true);
                    return;
                }
                RewardEpochDayData lastDayData;
                RewardEpochDayData currentDayData;
                for (int i = rewardEpoch.DayData.Count - 1; i > 0; i--)
                {
                    lastDayData = rewardEpoch.DayData[i - 1];
                    currentDayData = rewardEpoch.DayData[i];
                    if ($"Epoch_{rewardEpoch.EpochID}_Day_{currentDayData.DayID}" ==
                        progress.IngameDataProgress.LastClaimedRewardData.UniqueSaveID)
                    {
                        Debug.Log("Finished epoch");
                        currentDayData.State = RewardState.Claimed;
                        _currentEpoch.EpochFinished = true;
                        return;
                    }
                    if ($"Epoch_{rewardEpoch.EpochID}_Day_{lastDayData.DayID}" !=
                    progress.IngameDataProgress.LastClaimedRewardData.UniqueSaveID)
                    {
                        lastDayData.State = currentDayData.State = RewardState.Unclaimed;
                        continue;
                    }
                    lastDayData.State = RewardState.Claimed;
                    currentDayData.State = CanClaimReward
                        (progress.IngameDataProgress.LastClaimedRewardData.ClaimedDateTime);
                    if (currentDayData.State == RewardState.Unclaimed)
                        _nextRewardID = i;
                    else
                        _nextRewardID = i + 1;
                    FoundUnclaimedReward?.Invoke(currentDayData.State == RewardState.Claimable);
                    return;
                }
            }
        }

        private RewardState CanClaimReward(int claimedDateTime)
        {
            if (Epoch.CurrentRelativeTime(_currentEpoch.EpochStartDateTime.ToDateTime()) -
                (claimedDateTime + _staticData.GameData.EconomyStaticData.StandartRewardDayDuration.ToFloat())
                >= 0)
                return RewardState.Claimable;
            return RewardState.Unclaimed;
        }

        private async Task LoadConfig() =>
            _rewardsDataConfig = await _assetProvider.Load<DailyRewardsStaticData>
                (AssetAddress.DAILY_REWARDS_CONFIG);

        private void RegisterSaveableEntity() =>
            _mansionFactory.RegisterSaveableEntity((ISaveReader)this);

        private void Unsubscribe()
        {
            _projectTimeTracker.ChangedDay -= UpdateRewardsInRealTime;
        }
        private void OnDestroy()
        {
            Unsubscribe();
        }

    }
}
