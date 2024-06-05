using RoyalMasion.Code.Infrastructure.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoyalMansion.Code.UnityLogic.DailyRewards
{
	public class RewardDayView : MonoBehaviour
	{
		[SerializeField] private Image _icon;
		[SerializeField] private TextMeshProUGUI _dayNumber;
		[SerializeField] private TextMeshProUGUI _amount;

		[SerializeField] private Button _claimBtn;
		[SerializeField] private GameObject _checkMark;
		private RewardEpochDayData _dayData;

		public Action<RewardEpochDayData> RewardClaimed;

		public void Init(RewardEpochDayData dayData, int dayNumber)
		{
			_dayData = dayData;

			_icon.sprite = dayData.Icon;
			_dayNumber.text = dayNumber.ToString();
			_amount.text = dayData.Amount.ToString();
			_claimBtn.interactable = dayData.State == RewardState.Claimable;
			_checkMark.SetActive(dayData.State == RewardState.Claimed);
		}

		private void Start()
		{
			Subscribe();
		}
		private void Subscribe()
		{
			_claimBtn.onClick.AddListener(Claim);
		}

		private void Unsubscribe()
		{
			_claimBtn.onClick.RemoveListener(Claim);
		}

		private void Claim()
		{
			_claimBtn.interactable = false;
			_checkMark.SetActive(true);
			RewardClaimed?.Invoke(_dayData);
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

	}
}
