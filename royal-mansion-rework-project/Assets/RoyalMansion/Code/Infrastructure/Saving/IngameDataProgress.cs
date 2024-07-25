using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class IngameDataProgress
    {
        public List<GameResourceSaveData> ResourcesSaveData;
        public List<DailyMessagesSaveData> DailyMessagesSaveDatas;
        public DailyRewardSaveData LastClaimedRewardData;
        public void WriteResourceProgress(GameResourceSaveData data)
        {
            if (ResourcesSaveData == null)
            {
                ResourcesSaveData = new();
                ResourcesSaveData.Add(data);
                return;
            }
            for (int i = 0; i < ResourcesSaveData.Count; i++)
            {
                GameResourceSaveData itemData = ResourcesSaveData[i];
                if (itemData.ResourceType == data.ResourceType)
                {
                    ResourcesSaveData[i] = data;
                    return;
                }
            }
            ResourcesSaveData.Add(data);
        }
        public void TryAddMetaMessage(DailyMessagesSaveData targetMessageData)
        {
            if (DailyMessagesSaveDatas == null)
            {
                DailyMessagesSaveDatas = new();
                DailyMessagesSaveDatas.Add(targetMessageData);
                return;
            }
            for (int i = 0; i < DailyMessagesSaveDatas.Count; i++)
            {
                DailyMessagesSaveData messageData = DailyMessagesSaveDatas[i];
                if (messageData.UniqueSaveID == targetMessageData.UniqueSaveID)
                {
                    DailyMessagesSaveDatas[i] = targetMessageData;
                    return;
                }
            }
            DailyMessagesSaveDatas.Add(targetMessageData);
        }
        public void WriteClaimedRewardData(DailyRewardSaveData rewardData) => 
            LastClaimedRewardData = rewardData;
    }
}
