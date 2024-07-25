using RoyalMansion.Code.Extensions.Utils;
using RoyalMansion.Code.UI.Windows.DailyRewardWindow;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.DailyRewards
{
	[System.Serializable]
	public class RewardEpoch
	{
		public int EpochID;
		public List<RewardEpochDayData> DayData;
		public bool EpochFinished;
		public NormalizedDateTime EpochStartDateTime;
	}
	
}
