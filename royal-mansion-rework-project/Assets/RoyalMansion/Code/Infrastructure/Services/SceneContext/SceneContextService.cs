using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.ContextObjects.MansionContext;
using RoyalMansion.Code.UnityLogic.Meta;
using RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic;
using System;
using System.Collections.Generic;

namespace RoyalMasion.Code.Infrastructure.Services.SceneContext
{
    public class SceneContextService : ISceneContextService
    {
        public MansionSpawnPointData MansionSpawnPoints { get; set; }
        public Kitchen Kitchen { get; set; }
        public MansionCinemachineHandler CinemachineHandler { get; set; }
        public DailyMessagesHandler MetaMessagesHandler { get; set; }
    }
}