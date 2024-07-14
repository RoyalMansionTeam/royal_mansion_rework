using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using VContainer;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;

namespace RoyalMasion.Code.Infrastructure.Services.ProjectData
{
    public class EconomyDataService : IEconomyDataService
    {
        public Action<ResourceType, int> ResourceChanged { get; set; }

        private readonly IPersistentProgressService _progressService;

        private Dictionary<ResourceType, int> _resources;

        [Inject]
        public EconomyDataService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void InitEconomyService()
        {
            _resources = new();
            foreach(ResourceType type in Enum.GetValues(typeof(ResourceType)))
                _resources.Add(type, TryLoadProgress(type));
        }

        public int GetEconomyData(ResourceType dataType)
        {
            return _resources[dataType];
        }

        public void SetEconomyData(ResourceType dataType, int addAmount)
        {
            _resources[dataType] += addAmount;
            ResourceChanged?.Invoke(dataType, _resources[dataType]);
            SaveProgress(dataType);
        }

        public int TryLoadProgress(ResourceType type)
        {
            int amount = 2000;
            if (_progressService.Progress.IngameDataProgress.ResourcesSaveData == null)
                return amount;
            foreach(Saving.GameResourceSaveData resource in _progressService.Progress.IngameDataProgress.ResourcesSaveData)
            {
                if (resource.ResourceType != (int)type)
                    continue;
                amount = resource.Amount;
                break;
            }
            return amount;
        }

        public void SaveProgress(ResourceType dataType)
        {

            _progressService.Progress.IngameDataProgress.WriteResourceProgress(
                new Saving.GameResourceSaveData(
                    resourceType: (int)dataType,
                    amount: _resources[dataType]
                ));
        }

    }
}

