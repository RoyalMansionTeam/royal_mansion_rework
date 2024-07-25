using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.ContextObjects.MansionContext;
using RoyalMansion.Code.UnityLogic.DailyRewards;
using RoyalMansion.Code.UnityLogic.Meta;
using RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Services.SceneContext
{
    public interface ISceneContextService
    {
        MansionSpawnPointData MansionSpawnPoints { get; set; }
        Kitchen Kitchen { get; set; }
        MansionCinemachineHandler CinemachineHandler { get; set; }
        DailyMessagesHandler MetaMessagesHandler {get; set;}
        DailyRewardService DailyRewardService { get; set; }
    }
}