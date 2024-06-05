using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RoyalMansion.Code.UnityLogic.DailyRewards
{
	public class DailyRewardsPopupWindow : MonoBehaviour
	{
		[SerializeField] private Transform _rewardDaysParent;
		[SerializeField] private Button _closeBtn;

		private DailyRewardService _rewardService;
		private RewardEpoch _currentEpoch;

		private GameObject _dayViewPrefab;

		private void Awake()
		{
			_rewardService = new DailyRewardService();
			_rewardService.LoadRewardsData();
			//_dayViewPrefab = FlyBehaviour.Scene.Assets.GetPrefab<GameObject>(_rewardService.RewardsData.DayViewReference);
		}

		private void Start()
		{
			//Load calendar day prefab
			Subscribe();
			SpawnCalendarDays();
		}

		private void SpawnCalendarDays()
		{
			if (_rewardService.CurrentEpoch == null)
				return;
			_currentEpoch = _rewardService.CurrentEpoch;
			for (int i = 0; i < _currentEpoch.DayData.Count; i++)
			{
				GameObject dayView = Instantiate(_dayViewPrefab, _rewardDaysParent);
				var dayHandler = dayView.GetComponent<RewardDayView>();
				dayHandler.Init(_currentEpoch.DayData[i], i++);
				dayHandler.RewardClaimed += ClaimReward;
			}
		}

		private void Subscribe()
		{
			_closeBtn.onClick.AddListener(CloseWindow);
		}

		private void Unsubscribe()
		{
			_closeBtn.onClick.RemoveListener(CloseWindow);
		}

		private void CloseWindow()
		{
			Destroy(gameObject);
		}

		private void ClaimReward(RewardEpochDayData dayData)
		{
			//is subscribed to day claim btn (all days)
			//Handle resource and view logic
			//call UnpateDayView
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

	}

	[System.Serializable]
	public struct RewardEpochDayData
	{
		//public int Id; TODO: decide how to save reward data
		public ResourceType Type;
		public Sprite Icon;
		public int Amount;
		public RewardState State;
	}
}
