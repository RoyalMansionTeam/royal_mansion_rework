using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.Extensions.Utils
{
    public static class MansionUtils
    {
        public static Dictionary<Type, UnitState> StateCorrelation = new()
        {
            [typeof(LockedState)] = UnitState.Locked,
            [typeof(EmptyState)] = UnitState.Empty,
            [typeof(InUseState)] = UnitState.InUse,
            [typeof(CollectableState)] = UnitState.Collectable,
            [typeof(DirtyState)] = UnitState.Dirty
        };

    }
}
