using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System.Threading.Tasks;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public interface INpcFactory
    {
        TNpc SpawnNpc<TNpc>() where TNpc : NpcBase;
        TNpc SpawnNpc<TNpc>(Transform at) where TNpc : NpcBase;
        Task SetNpcFactory();
        void Clear();
        TNpc SpawnNpc<TNpc>(Vector3 at) where TNpc : NpcBase;
    }
}
