using RoyalMansion.Code.UnityLogic.ContextObjects.MansionContext;
using RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic;
using System;
using System.Collections.Generic;

namespace RoyalMasion.Code.Infrastructure.Services.SceneContext
{
    public class SceneContextService : ISceneContextService
    {
        public MansionSpawnPointData MansionSpawnPoints { get; set; }
        public Kitchen Kitchen { get; set; }
    }
}