using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace RoyalMansion.Code.UnityLogic.Meta
{
    public class DailyMessagesHandler : MonoBehaviour, ISaveWriter, ISaveReader
    {
        public string SaveableID { get; set; }
        public Action<DailyMessageData> AvailableMessageFound;

        private IAssetProvider _assetProvider;
        private IMansionFactory _mansionFactory;
        private IStaticDataService _staticData;
        private DailyMessagesConfig _messagesConfig;


        //Load configs - DONE
        //Load save data ?and apply to config?
        //check if already checked in
        //get today message if available and invoke corr. event
        //event is listened from FailyMessagesHUD by SceneContext

        [Inject]
        public void Construct(IAssetProvider assetProvider, IMansionFactory mansionFactory,
            IStaticDataService staticData)
        {
            _assetProvider = assetProvider;
            _mansionFactory = mansionFactory;
            _staticData = staticData;
            RegisterSaveableEntity();
            Debug.Log("Register");
        }

        public void LoadProgress(GameProgress progress)
        {
            SetupData(progress);
        }


        public void SaveProgress(GameProgress progress)
        {
            if (_messagesConfig == null)
                return;
            foreach (DailyMessageData messageSequence in _messagesConfig.DailyMessages)
            {
                progress.MansionProgress.TryAddMetaMessage(new DailyMessagesSaveData(
                    uniqueSaveID: messageSequence.SequenceID,
                    seuenceRead: messageSequence.SequenceRead,
                    claimedDateTime: messageSequence.ClaimedDateTime
                    ));
            }
        }
        private async void SetupData(GameProgress progress)
        {
            await LoadConfig();
            if (_messagesConfig == null)
                return;
            if (progress == null || progress.MansionProgress.DailyMessagesSaveDatas == null)
            {
                TrySetDailyMessage();
                return;
            }
            foreach (DailyMessageData messageSequence in _messagesConfig.DailyMessages)
            {
                foreach (DailyMessagesSaveData savedMessage in progress.MansionProgress.DailyMessagesSaveDatas)
                {
                    if (savedMessage.UniqueSaveID != messageSequence.SequenceID)
                        continue;
                    messageSequence.SequenceRead = savedMessage.SequenceRead;
                    messageSequence.ClaimedDateTime = savedMessage.ClaimedDateTime;
                    break;
                }
            }
            TrySetDailyMessage();
        }

        private async Task LoadConfig() =>
            _messagesConfig = await _assetProvider.Load<DailyMessagesConfig>
            (AssetAddress.META_MESSAGES_CONFIG_DATA_PATH);

        private void TrySetDailyMessage()
        {
            DailyMessageData lastMessageSequence = null;
            for(int i = _messagesConfig.DailyMessages.Count-1; i>=0; i--)
            {
                DailyMessageData messageSequence = _messagesConfig.DailyMessages[i];
                if (i == 0 || messageSequence.SequenceRead)
                {
                    AvailableMessageFound?.Invoke(messageSequence);
                    return;
                }
                lastMessageSequence = _messagesConfig.DailyMessages[i - 1];
                if (!messageSequence.SequenceRead & lastMessageSequence.SequenceRead)
                {
                    if (IsTodaysMessage(lastMessageSequence.ClaimedDateTime))
                    {
                        AvailableMessageFound?.Invoke(lastMessageSequence);
                        return;
                    }
                    AvailableMessageFound?.Invoke(messageSequence);
                    return;
                }
            }
            /*foreach (DailyMessageData messageSequence in _messagesConfig.DailyMessages)
            {
                if (messageSequence.SequenceRead)
                {
                    lastMessageSequence = messageSequence;
                    continue;
                }
                if (lastMessageSequence == null)
                {
                    AvailableMessageFound?.Invoke(messageSequence);
                    return;
                }
                if (IsTodaysMessage(lastMessageSequence.ClaimedDateTime))
                {
                    AvailableMessageFound?.Invoke(lastMessageSequence);
                    return;
                }
                AvailableMessageFound?.Invoke(messageSequence);
                return;
            }*/
        }

        private bool IsTodaysMessage(string claimedDateTime)
        {
            if (DateTime.TryParse(claimedDateTime, out DateTime messageClaimedDateTime))
            {
                return messageClaimedDateTime.Day == DateTime.Today.Day;
            }
            return false;
        }

        private void RegisterSaveableEntity() =>
            _mansionFactory.RegisterSaveableEntity((ISaveReader)this);

    }
}
