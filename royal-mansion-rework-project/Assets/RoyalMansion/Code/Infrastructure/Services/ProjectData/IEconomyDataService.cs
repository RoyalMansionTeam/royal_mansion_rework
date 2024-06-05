using RoyalMasion.Code.Infrastructure.Data;
using System;

namespace RoyalMasion.Code.Infrastructure.Services.ProjectData
{
    public interface IEconomyDataService
    {
        Action<ResourceType, int> ResourceChanged { get; set; }
        int GetEconomyData(ResourceType dataType);
        void InitEconomyService();
        void SetEconomyData(ResourceType dataType, int addAmount);
    }
}

