using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.StaticData
{
    [Serializable]
    public class EconomyStaticData
    {
        [SerializeField] private NormalizedTime _standartRewardDayDuration;
        public List<RecruitmentPriceData> StaffRecruitmentPrices;
        public List<ValueIconData> ValueIcons;
        public Sprite GetIcon(ResourceType type)
        {
            Sprite icon = null;
            foreach(ValueIconData valueData in ValueIcons)
            {
                if (valueData.Type != type)
                    continue;
                icon = valueData.Icon;
                break;
            }
            return icon;
        }
        public RecruitmentPriceData GetRecruitmentPrice(NpcType staffType)
        {
            RecruitmentPriceData data = null;
            foreach(var priceData in StaffRecruitmentPrices)
            {
                if (priceData.StaffType != staffType)
                    continue;
                data = priceData;
                break;
            }
            return data;
        }
        public NormalizedTime StandartRewardDayDuration => _standartRewardDayDuration;
    }
    [Serializable]
    public class RecruitmentPriceData
    {
        public NpcType StaffType;
        public ResourceType ValueType;
        public int Amount;
    }

    [Serializable]
    public class ValueIconData
    {
        public ResourceType Type;
        public Sprite Icon;
    }
}