using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.DailyRewards
{
	//Note: RewardEpoch was made as a class instead of a struct so that it can be referenced as a Null type
	[System.Serializable]
	public class RewardEpoch
	{
		public NormalizedDateTime StartDateTime;
		public NormalizedDateTime EndDateTime;
		public List<RewardEpochDayData> DayData;

		public void SetRewardState(RewardState state)
		{

		}
		
	}
	
}
