using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [Serializable]
    public class MansionProgress
    {
        public List<CatalogItemSaveData> CatalogItemsSave;
        public List<MansionUnitSaveData> MansionUnitsSave;
        public List<TimerSaveData> TimerSaveData;
        public List<NpcSaveData> NpcSaveData;

        public void TryAddItem(CatalogItemSaveData targetItem)
        {
            if (CatalogItemsSave == null)
            {
                CatalogItemsSave = new();
                CatalogItemsSave.Add(targetItem);
                return;
            }
            for (int i = 0; i < CatalogItemsSave.Count; i++)
            {
                CatalogItemSaveData itemData = CatalogItemsSave[i];
                if (itemData.UniqueSaveID == targetItem.UniqueSaveID)
                {
                    CatalogItemsSave[i] = targetItem;
                    return;
                }
            }
            CatalogItemsSave.Add(targetItem);
        }
        public void TryAddUnit(MansionUnitSaveData unitData)
        {
            if (MansionUnitsSave == null)
            {
                MansionUnitsSave = new();
                MansionUnitsSave.Add(unitData);
                return;
            }
            for (int i = 0; i < MansionUnitsSave.Count; i++)
            {
                MansionUnitSaveData itemData = MansionUnitsSave[i];
                if (itemData.UniqueSaveID == unitData.UniqueSaveID)
                {
                    MansionUnitsSave[i] = unitData;
                    return;
                }
            }
            MansionUnitsSave.Add(unitData);
        }
        public void TryAddTimer(TimerSaveData targetTimer)
        {
            if (TimerSaveData == null)
            {
                TimerSaveData = new();
                TimerSaveData.Add(targetTimer);
                return;
            }
            for (int i = 0; i < TimerSaveData.Count; i++)
            {
                TimerSaveData timerData = TimerSaveData[i];
                if (timerData.UniqueSaveID == targetTimer.UniqueSaveID)
                {
                    TimerSaveData[i] = targetTimer;
                    return;
                }
            }
            TimerSaveData.Add(targetTimer);
        }

        public void TryAddNpc(NpcSaveData npc)
        {
            if (NpcSaveData == null)
            {
                NpcSaveData = new();
                NpcSaveData.Add(npc);
                return;
            }
            for (int i = 0; i < CatalogItemsSave.Count; i++)
            {
                NpcSaveData npcData = NpcSaveData[i];
                if (npcData.UniqueSaveID == npc.UniqueSaveID)
                {
                    NpcSaveData[i] = npc;
                    return;
                }
            }
            NpcSaveData.Add(npc);
        }
        public void TryRemoveNpc(string uniqueID)
        {
            if (NpcSaveData == null)
                return;
            for (int i = 0; i < CatalogItemsSave.Count; i++)
            {
                NpcSaveData npcData = NpcSaveData[i];
                if (npcData.UniqueSaveID == uniqueID)
                {
                    NpcSaveData.Remove(npcData);
                    return;
                }
            }
        }
    }
}
