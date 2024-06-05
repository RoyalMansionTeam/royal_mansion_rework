using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoyalMasion.Code.Infrastructure.Data;
using System;

namespace RoyalMasion.Code.Infrastructure.Services.ProjectData
{
    public class EconomyDataService : IEconomyDataService
    {
        public Action<ResourceType, int> ResourceChanged { get; set; }

        private Dictionary<ResourceType, int> _resources;
        public void InitEconomyService()
        {
            _resources = new();
            foreach(ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                _resources.Add(type, 0); //TODO: Insert saved resource value here later
            }
        }

        public int GetEconomyData(ResourceType dataType)
        {
            return _resources[dataType];
        }

        public void SetEconomyData(ResourceType dataType, int addAmount)
        {
            _resources[dataType] += addAmount;
            ResourceChanged?.Invoke(dataType, _resources[dataType]);
        }
    }
}

