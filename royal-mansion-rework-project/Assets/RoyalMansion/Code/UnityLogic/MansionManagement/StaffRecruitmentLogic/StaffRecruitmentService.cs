using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMasion.Code.Infrastructure.StaticData;
using RoyalMasion.Code.UI.Windows;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.StaffRecruitmentLogic
{
    public class StaffRecruitmentService : MonoBehaviour
    {
        public Action<NpcType> StaffRecruited;
        private IUIFactory _uiFactory;
        private INpcFactory _npcFactory;
        private IStaticDataService _staticData;
        private ISceneContextService _sceneContext;

        [Inject]
        public void Construct(IUIFactory uiFactory, INpcFactory npcFactory, IStaticDataService staticData,
            ISceneContextService sceneContext)
        {
            _uiFactory = uiFactory;
            _npcFactory = npcFactory;
            _staticData = staticData;
            _sceneContext = sceneContext;
        }

        public void SpawnRecruitmentWindow(RecruitmentMessageData messageData) 
        {
            StaffRecruitmentWindow recruitmentPopUp = _uiFactory.CreateWindow(WindowID.StaffRecruitment).GetComponent<StaffRecruitmentWindow>();
            recruitmentPopUp.SetWindowData(
                messageData: messageData,
                priceData: _staticData.GameData.EconomyStaticData.GetRecruitmentPrice(messageData.Type));
            recruitmentPopUp.StaffMemberRecruited += ValidateRecruitedStaff;
        }

        private void ValidateRecruitedStaff(NpcType staffType)
        {
            switch(staffType)
            {
                case NpcType.Cook:
                    CookNPC cookNpc = _npcFactory.SpawnNpc<CookNPC>(_sceneContext.Kitchen.CookSpawnPoint);
                    cookNpc.SetNPCData(_uiFactory,
                        _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
                    _sceneContext.Kitchen.AddCook(cookNpc);
                    break;
                case NpcType.Maid:
                    MaidNPC maidNpc = _npcFactory.SpawnNpc<MaidNPC>(_sceneContext.MansionSpawnPoints.GuestSpawnPoint);
                    maidNpc.SetNPCData(_uiFactory,
                        _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
                    _sceneContext.MaidService.AddMaid(maidNpc);
                    break;
            }
            StaffRecruited?.Invoke(staffType);
        }
    }  
}