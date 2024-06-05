using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public interface INpcFactory
    {
        TNpc SpawnNpc<TNpc>() where TNpc : NpcBase;
        void SetNpcFactory();
        void Clear();
    }
}
