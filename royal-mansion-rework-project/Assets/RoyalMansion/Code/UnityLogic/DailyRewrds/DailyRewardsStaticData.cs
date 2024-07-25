using RoyalMansion.Code.Extensions.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMansion.Code.UnityLogic.DailyRewards
{
    [CreateAssetMenu(fileName = "DailyRewardsConfig", menuName = "Economy/DailyRewardsConfig")]
    public class DailyRewardsStaticData : ScriptableObject
    {
        [SerializeField] private List<RewardEpoch> _rewardsEpochs;
 
        public List<RewardEpoch> RewardsEpochs => _rewardsEpochs;


        /// <summary>
        /// Removes passed  epochs from a project. Build mode ONLY
        /// </summary>
        public void DestroyPastEpochs()
        {
#if !UNITY_EDITOR
			//TODO: Place logic for clearing a list of epochData with epochs that already have passed (only for build ver.)
#endif
        }
    }

}
