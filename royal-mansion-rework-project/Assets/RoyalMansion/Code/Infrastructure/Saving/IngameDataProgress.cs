using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class IngameDataProgress
    {
        public List<GameResourceSaveData> ResourcesSaveData;
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
    }
}
