
using System;

namespace RoyalMansion.Code.UnityLogic.DailyRewards
{
	public class DailyRewardService
	{
		private DailyRewardsStaticData _rewardsData;
		private RewardEpoch _currentEpoch;

		public RewardEpoch CurrentEpoch => _currentEpoch;
		public DailyRewardsStaticData RewardsData => _rewardsData;

		public void LoadRewardsData()
		{
			//_rewardsData = StaticInstances<DailyRewardsStaticData>.GetInstance();
			if (TryGetEpoch(DateTime.UtcNow, out RewardEpoch epoch)!=null)
				_currentEpoch = epoch;
		}

		private RewardEpoch TryGetEpoch(DateTime targetDate, out RewardEpoch epoch)
		{
			epoch = null;
			foreach (RewardEpoch epochData in _rewardsData.RewardsEpochs)
			{
				if (targetDate >= epochData.StartDateTime.ToDateTime() &
					targetDate <= epochData.EndDateTime.ToDateTime())
					epoch = epochData;

			}
			return epoch;
		}
	}
}
