using UnityEngine;
using UnityEngine.UI;
using RoyalMasion.Code.Infrastructure.Data;
using System.Collections.Generic;
using VContainer;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using System;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic
{
    public class MansionUnit : MonoBehaviour
    {
        [SerializeField] protected Button UnitActionBtn;
        protected IMansionStateMashine StateMashine;
        protected INpcFactory _npcFactory;
        [SerializeField] private Transform _timerParent;
        [SerializeField] private Transform _timerPosition;
        [SerializeField] private Transform _newItemSpawnPoint;

        private ISceneContextService _sceneContext;
        private IUIFactory _uiFactory;
        private IEconomyDataService _econommyDataService;
        private IMansionFactory _mansionFactory;
        private MansionStateMachineData StateMashineData;
        private UnitStaticData UnitData;
        private TimerSpawnData TimerSpawn;


        [Inject]
        public void Construct(IEconomyDataService dataService,
            IMansionFactory mansionFactory, INpcFactory npcFactory,
            ISceneContextService sceneContext, IUIFactory uiFactory)
        {
            _econommyDataService = dataService;
            _mansionFactory = mansionFactory;
            _npcFactory = npcFactory;
            _sceneContext = sceneContext;
            _uiFactory = uiFactory;
        }

        public void InitUnitData(UnitStaticData unitData)
        {
            TimerSpawn = new TimerSpawnData(_timerPosition.position, _timerParent);
            UnitData = unitData;
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            StateMashineData = new MansionStateMachineData
                (
                    mansionFactory: _mansionFactory,
                    economyDataService: _econommyDataService,
                    npcFactory: _npcFactory,
                    timerSpawnData: TimerSpawn,
                    unitStaticData: UnitData,
                    sceneContext: _sceneContext,
                    uiFactory: _uiFactory,
                    itemSpawnPoint: _newItemSpawnPoint
                );
            StateMashine = new MansionStateMashine(StateMashineData);
        }
    }

}
