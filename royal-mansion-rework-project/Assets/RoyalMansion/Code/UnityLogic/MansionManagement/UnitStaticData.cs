using RoyalMansion.Code.Extensions.Utils;
using RoyalMansion.Code.UnityLogic.DailyRewards;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic
{
    public class UnitStaticData : ScriptableObject
    {
        [SerializeField] public string UnitID;
        [SerializeField] private UnitTaskData[] _tasks;
        [SerializeField] private UnitType _unitType;
        [SerializeField] private int _unitPrice;
        [SerializeField] private UnitFurnitureRequirements[] _basicRequirements;

        public UnitType UnitType => _unitType;
        public int UnitPrice => _unitPrice;
        //public UnitFurnitureRequirements[] BasicRequirements => _basicRequirements;
        public UnitTaskData GetTaskData(UnitState targetState)
        {
            foreach (var task in _tasks)
            {
                if (task.State == targetState)
                    return task;
            }
            return null;
        }
        public UnitFurnitureRequirements[] GetUnitBasicRequirements()
        {
            UnitFurnitureRequirements[] result = new UnitFurnitureRequirements[_basicRequirements.Length];
            for (int i = 0; i < _basicRequirements.Length; i++)
                result[i] = new UnitFurnitureRequirements() { 
                    ItemType = _basicRequirements[i].ItemType, 
                    Amount = _basicRequirements[i].Amount };
            return result;
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

    [Serializable]
    public class UnitFurnitureRequirements
    {
        public CatalogSection ItemType;
        public int Amount;
    }
}
