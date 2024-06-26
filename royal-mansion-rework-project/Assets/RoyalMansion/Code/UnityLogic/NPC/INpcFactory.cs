using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System.Threading.Tasks;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public interface INpcFactory
    {
        TNpc SpawnNpc<TNpc>() where TNpc : NpcBase;
        Task SetNpcFactory();
        void Clear();
    }
}
