using RoyalMansion.Code.Extensions;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using System;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.Services.ProjectData
{
    public class ProjectTimeTracker : MonoBehaviour
    {
        public Action ChangedDay;
        public float TimeTillNextDay;

        private DateTime _epochStartTime;
        private int _projectDayDuration;
        private bool _ableToTrackEpoch = false;
        private int _nextUpdate = 1;

        public void SetEpochToTrack(DateTime startTime, int dayDuration)
        {
            _projectDayDuration = dayDuration;
            _epochStartTime = startTime;
            _ableToTrackEpoch = true;
            TimeTillNextDay = _projectDayDuration - ((int)Epoch.CurrentRelativeTime(_epochStartTime) % _projectDayDuration);
            Debug.Log($"Time upon load: Day duration: {TimeSpan.FromSeconds(_projectDayDuration)}, " +
                $"%: {(int)Epoch.CurrentRelativeTime(_epochStartTime) % _projectDayDuration},  " +
                $"Calculated time: {TimeSpan.FromSeconds(TimeTillNextDay)}");
        }


        void Update()
        {
            if (_projectDayDuration == 0)
                return;
            if (!_ableToTrackEpoch)
                return;
            if (Time.time >= _nextUpdate)
            {
                _nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                UpdatePerSecond();
            }
            if (TimeTillNextDay <= 0)
                return;
            TimeTillNextDay -= Time.deltaTime;
        }


        private void UpdatePerSecond()
        {
            if ((int)Epoch.CurrentRelativeTime(_epochStartTime) % _projectDayDuration == 0)
            {
                ChangedDay?.Invoke();
                TimeTillNextDay = _projectDayDuration;
            }

        }
    }
}

