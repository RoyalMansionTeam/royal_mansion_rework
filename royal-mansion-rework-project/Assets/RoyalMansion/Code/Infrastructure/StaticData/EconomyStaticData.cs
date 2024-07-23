using RoyalMasion.Code.Infrastructure.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.StaticData
{
    [Serializable]
    public class EconomyStaticData
    {
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
    }

    [Serializable]
    public class ValueIconData
    {
        public ResourceType Type;
        public Sprite Icon;
    }
}