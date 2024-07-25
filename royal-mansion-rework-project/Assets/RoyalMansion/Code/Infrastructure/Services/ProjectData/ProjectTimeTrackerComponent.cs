using RoyalMasion.Code.Infrastructure.Services.StaticData;
using System;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.Services.ProjectData
{
    public class ProjectTimeTrackerComponent : MonoBehaviour
    {
        private ProjectTimeTracker _timeTracker;
        private IStaticDataService _staticData;

        [Inject]
        public void Construct(ProjectTimeTracker timeTracker, IStaticDataService staticData)
        {
            _timeTracker = timeTracker;
            _staticData = staticData;
        }

        public void SetTimeTrackerEpoch(DateTime startTime) =>
            _timeTracker.SetEpochToTrack(startTime, (int)_staticData.GameData.EconomyStaticData.StandartRewardDayDuration.ToFloat());
    }
}

