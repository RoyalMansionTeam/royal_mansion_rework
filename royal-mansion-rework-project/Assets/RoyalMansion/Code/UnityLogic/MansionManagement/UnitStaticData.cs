using RoyalMansion.Code.Extensions.Utils;
using RoyalMansion.Code.UnityLogic.DailyRewards;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic
{
    public class UnitStaticData : ScriptableObject
    {
        [SerializeField] private UnitTaskData[] _tasks;
        [SerializeField] private UnitType _unitType;

        public UnitType UnitType => _unitType;
        public UnitTaskData GetTaskData(UnitState targetState)
        {
            foreach (var task in _tasks)
            {
                if (task.State == targetState)
                    return task;
            }
            return null;
        }

    }

    [Serializable]
    public class UnitTaskData
    {
        public UnitState State;
        public NormalizedTime Time;
        public ResourceType Reward;
        public int Amount;
    }
}
