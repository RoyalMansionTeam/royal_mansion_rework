using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.StaffRecruitmentLogic
{
    public class MaidService : MonoBehaviour, ISaveReader
    {
        private const string Unit_Name = "MaidService";

        [SerializeField] private Transform _maidRestRoom;
        public Action<MaidNPC> AvailableMaidFound;

        private IAssetProvider _assetProvider;
        private ISceneContextService _sceneContext;
        private INpcFactory _npcFactory;
        private IUIFactory _uiFactory;
        private IMansionFactory _mansionFactory;

        private RecruitmentMessagesConfig _recruitmentConfig;
        private RecruitmentMessageData _maidMessageData;
        private List<MaidNPC> _maids = new();
        private MaidNPC _targetMaid;
        private Transform _lastCalledRoomPosition;

        [Inject]
        public void Construct(IAssetProvider assetProvider, ISceneContextService sceneContext,
            INpcFactory npcFactory, IUIFactory uIFactory, IMansionFactory mansionFactory)
        {
            _assetProvider = assetProvider;
            _sceneContext = sceneContext;
            _npcFactory = npcFactory;
            _uiFactory = uIFactory;
            _mansionFactory = mansionFactory;
            RegisterSaveableEntity();
        }
        public void LoadProgress(GameProgress progress)
        {
            if (progress == null || progress.MansionProgress.NpcSaveData == null)
                return;
            foreach (NpcSaveData saveData in progress.MansionProgress.NpcSaveData)
            {
                if (saveData.AssignedUnitID != Unit_Name)
                    continue;
                MaidNPC npc;
                if (saveData.State == (int)NpcState.Resting)
                    npc = _npcFactory.SpawnNpc<MaidNPC>(_maidRestRoom);
                else
                    npc = _npcFactory.SpawnNpc<MaidNPC>(saveData.TargetPosition.AsUnityVector());
                npc.SetNPCData(_uiFactory,
                    _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
                npc.TargetUnitID = saveData.TargetUnitID;
                npc.TargetPosition = saveData.TargetPosition.AsUnityVector();
                npc.State = saveData.State;
                npc.SaveableID = saveData.UniqueSaveID;
                AddMaid(npc);
                npc.SetTarget(npc.TargetPosition);
                if (npc.State == (int)NpcState.PerformingTask)
                {
                    AvailableMaidFound?.Invoke(npc);
                }
            }
        }

        private void Start()
        {
            LoadRecruitmentMessage();
        }

        public void AddMaid(MaidNPC npc)
        {
            npc.AssignedUnitID = Unit_Name;
            _maids.Add(npc);
        }

        public MaidNPC TryAssignMaid(Transform targetUnitPosition, string targetUnitID)
        {
            _lastCalledRoomPosition = targetUnitPosition;
            MaidNPC accessableMaid = null;
            if (_maids.Count != 0)
            {
                FindAvailableNPC(NpcType.Maid);
                accessableMaid = _targetMaid;
                accessableMaid.TargetUnitID = targetUnitID;
            }
            else
                PlaceRecruitmentWindow();
            return accessableMaid;
        }

        public void OnNpcTargetAchieved(NpcBase npc)
        {
            if (npc.AssignedUnitID != Unit_Name)
                return;
            npc.gameObject.GetComponent<MaidNPC>().OnTargetAchieved();
        }

        public void SendMaidBack(MaidNPC targetMaid)
        {
            targetMaid.SetTarget(_maidRestRoom.position);
            targetMaid.EnterMaidBehaviour(NpcState.Resting);
        }

        /*public MaidNPC TryFindAssignedMaid(string unitID)
        {
            Debug.Log(_maids.Count);
            if (_maids.Count == 0)
                return null;
            MaidNPC assignedMaid = null;
            foreach (MaidNPC maid in _maids)
            {
                Debug.Log(maid.TargetUnitID);
                if (maid.TargetUnitID != unitID)
                    continue;
                assignedMaid = maid;
                break;
            }
            return assignedMaid;
        }*/

        private void PlaceRecruitmentWindow()
        {
            _sceneContext.StaffRecruitmentService.SpawnRecruitmentWindow(_maidMessageData);
            _sceneContext.StaffRecruitmentService.StaffRecruited += FindAvailableNPC;
        }

        private void FindAvailableNPC(NpcType calledType)
        {
            if (calledType != NpcType.Maid)
                return;
            _targetMaid = null;
            foreach (MaidNPC maid in _maids)
            {
                if (maid.CurrentBehavior.State == NpcState.PerformingTask)
                    continue;
                _targetMaid = maid;
                break;
            }
            if (_targetMaid != null)
            {
                SetMaidTask(_targetMaid);
                AvailableMaidFound?.Invoke(_targetMaid);
            }
        }
        private void SetMaidTask(MaidNPC maid)
        {
            maid.SetTarget(_lastCalledRoomPosition.position);
            maid.EnterMaidBehaviour(NpcState.PerformingTask);
        }


        private async void LoadRecruitmentMessage()
        {
            _recruitmentConfig = await _assetProvider.Load<RecruitmentMessagesConfig>
                            (AssetAddress.RECRUITMENT_MESSAGES_CONFIG);
            foreach (RecruitmentMessageData message in _recruitmentConfig.MessageDatas)
            {
                if (message.Type != NpcType.Maid)
                    continue;
                _maidMessageData = message;
                break;
            }
        }

        private void RegisterSaveableEntity() =>
            _mansionFactory.RegisterSaveableEntity(this);

    }
}