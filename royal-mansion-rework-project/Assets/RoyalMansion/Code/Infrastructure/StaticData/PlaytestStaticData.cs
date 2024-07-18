using RoyalMansion.Code.Extensions.Utils;
using System;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.StaticData
{
    [Serializable]
    public class PlaytestStaticData
    {
        [SerializeField] public int WaiterNumber;
        [SerializeField] public int CookNumber;
        [SerializeField] public int PantryCapacity;
        [SerializeField] public KitchenOrderData TestOrderData;
    }

    [Serializable]
    public class KitchenOrderData
    {
        public NormalizedTime OrderMakingTime;
        public int OrderReward;
        public int OrderResourceCost;
    }
}
