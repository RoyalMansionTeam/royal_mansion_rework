using RoyalMansion.Code.Extensions;
using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.DailyRewards;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.UI.Windows;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace RoyalMansion.Code.UI.Windows.DailyRewardWindow
{
    public class DailyRewardsWindow : WindowBase
    {
        private const string ClaimRewardText = "Claim now!";
        private const string UnclaimableRewardText = "Come back in: ";

        [SerializeField] private Transform _rewardDaysParent;
        [SerializeField] private AssetReference _dayReference;
        [SerializeField] private TextUIHandler _messageTextHandler;
        [SerializeField] private TextUIHandler _dayDurationTextHandler;
        [SerializeField] private GameObject _eventOverPopUp;

        public Action RewardClaimed;

        private DailyRewardService _rewardService;
        private RewardEpoch _currentEpoch;
        private List<RewardDayView> _viewHandlers = new();

        private GameObject _dayViewPrefab;
        private IEconomyDataService _economyData;
        private IMansionFactory _mansionFactory;
        private ISceneContextService _sceneContext;
        private IStaticDataService _staticData;
        private ProjectTimeTracker _timeTracker;

        private TimeSpan _targetRewardTime;
        private bool _displayRemainingTime;
        private float _targetRewardSeconds;
        private int _nextDayRewardID;

        [Inject]
        public void Construct(IEconomyDataService economyData, IMansionFactory mansionFactory,
            ISceneContextService sceneContext, IStaticDataService staticData, ProjectTimeTracker timeTracker)
        {
            _economyData = economyData;
            _mansionFactory = mansionFactory;
            _sceneContext = sceneContext;
            _staticData = staticData;
            _timeTracker = timeTracker;
        }

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _timeTracker.ChangedDay += UpdateRewardInRealTime;
        }

        private void UpdateRewardInRealTime()
        {
            if (_viewHandlers.Count <= _nextDayRewardID)
                return;
            if (_viewHandlers[_nextDayRewardID - 1].State != RewardState.Claimed)
                return;
            _viewHandlers[_nextDayRewardID].SetState(RewardState.Claimable);
            _targetRewardSeconds = 0;
            _messageTextHandler.SetTextField(ClaimRewardText);
            _nextDayRewardID++;
        }

        private void Update()
        {
            if (!_displayRemainingTime)
                return;
            if (_targetRewardSeconds <= 0)
                return;
            _targetRewardSeconds = _timeTracker.TimeTillNextDay;
            DisplayRemainingTime();
        }
        public void SetRewardWindow(RewardEpoch targetEpoch)
        {
            _rewardService = _sceneContext.DailyRewardService;
            _dayDurationTextHandler.SetTextField($"1 day = {_staticData.GameData.EconomyStaticData.StandartRewardDayDuration.ToDateTimeString()}");
            _currentEpoch = targetEpoch;
            if (_currentEpoch == null)
                return;
            if (_currentEpoch.EpochFinished)
            {
                LastDayClaimed(null);
                return;
            }
            SpawnCalendarDays();
        }

        private async void SpawnCalendarDays()
        {
            for (int i = 0; i < _currentEpoch.DayData.Count; i++)
            {
                GameObject dayView = await _mansionFactory.CreateUnitObject
                                (_dayReference.AssetGUID, transform.position, _rewardDaysParent);
                RewardDayView dayHandler = dayView.GetComponent<RewardDayView>();
                _viewHandlers.Add(dayHandler);
                dayHandler.Init(_currentEpoch.DayData[i], i + 1);
                dayHandler.RewardClaimed += ClaimReward;
                if (_currentEpoch.DayData[i].State == RewardState.Claimable)
                    _messageTextHandler.SetTextField(ClaimRewardText);          
                if (i < 1)
                    continue;
                if (_currentEpoch.DayData[i - 1].State == RewardState.Claimable)
                    _nextDayRewardID = i;
                if (_currentEpoch.DayData[i - 1].State == RewardState.Claimed)
                {
                    _targetRewardSeconds = (_currentEpoch.DayData[i - 1].ClaimedDateTime +
                        _staticData.GameData.EconomyStaticData.StandartRewardDayDuration.ToFloat()) -
                        Epoch.CurrentRelativeTime(_currentEpoch.EpochStartDateTime.ToDateTime());
                    DisplayRemainingTime();
                    _nextDayRewardID = i;
                    _displayRemainingTime = true;
                }
                if (i == _currentEpoch.DayData.Count - 1)
                    dayHandler.RewardClaimed += LastDayClaimed;
            }
        }

        private void LastDayClaimed(RewardEpochDayData dayData)
        {
            _currentEpoch.EpochFinished = true;
            Ubnsubscribe();
            _messageTextHandler.SetTextField("");
            _eventOverPopUp.SetActive(true);
        }

        private void ClaimReward(RewardEpochDayData dayData)
        {
            RewardClaimed?.Invoke();
            dayData.State = RewardState.Claimed;
            dayData.ClaimedDateTime = (int)Epoch.CurrentRelativeTime(_currentEpoch.EpochStartDateTime.ToDateTime());
            _rewardService.UpdateClaimedReward(
                $"Epoch_{_currentEpoch.EpochID}_Day_{dayData.DayID}",
                dayData.ClaimedDateTime);
            _economyData.SetEconomyData(dayData.Type, dayData.Amount);

            _targetRewardSeconds = _staticData.GameData.EconomyStaticData.StandartRewardDayDuration.ToFloat();
            DisplayRemainingTime();
            _displayRemainingTime = true;
        }
        private void DisplayRemainingTime()
        {
            _targetRewardTime = TimeSpan.FromSeconds(_targetRewardSeconds);
            _messageTextHandler.SetTextField
                ($"{UnclaimableRewardText} {string.Format("{0:D2}:{1:D2}:{2:D2}", _targetRewardTime.Hours, _targetRewardTime.Minutes, _targetRewardTime.Seconds)}");
        }
        private void Ubnsubscribe()
        {
            _timeTracker.ChangedDay -= UpdateRewardInRealTime;
        }

        private void OnDestroy()
        {
            Ubnsubscribe();
        }

    }

    [System.Serializable]
    public class RewardEpochDayData
    {
        public int DayID;
        public ResourceType Type;
        public Sprite Icon;
        public int Amount;
        public RewardState State;
        public int ClaimedDateTime;
    }
}

