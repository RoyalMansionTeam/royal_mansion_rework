using UnityEngine;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using RoyalMasion.Code.Infrastructure.Saving;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine
{
    public class MansionStateMachineData
    {
        public ISceneContextService SceneContext { get; }
        public IEconomyDataService EconomyData { get; }
        public INpcFactory NpcFactory { get; }
        public UnitStaticData UnitData { get; }
        public IUIFactory UiFactory { get; }
        public MansionUnitUIHandler UnitUIHandler { get;}
        public Transform ItemSpawnPoint { get; }
        public Transform NavMeshTarget { get; }
        public Action<CatalogSection> ItemBoughtEvent { get; }
        public Action BasicRequirementsMet { get; set; }
        public NpcSaveData NpcSaveData { get; }

        public MansionStateMachineData(IEconomyDataService economyDataService,
            INpcFactory npcFactory, UnitStaticData unitStaticData,
            ISceneContextService sceneContext, IUIFactory uiFactory, Transform itemSpawnPoint,
            Transform navMeshTarget, MansionUnitUIHandler unitUIHandler,
            Action<CatalogSection> itemBoughtEvent, NpcSaveData npcSaveData)
        {
            EconomyData = economyDataService;
            NpcFactory = npcFactory;
            UnitData = unitStaticData;
            SceneContext = sceneContext;
            UiFactory = uiFactory;
            ItemSpawnPoint = itemSpawnPoint;
            NavMeshTarget = navMeshTarget;
            UnitUIHandler = unitUIHandler;
            ItemBoughtEvent = itemBoughtEvent;
            NpcSaveData = npcSaveData;
        }

    }
}
