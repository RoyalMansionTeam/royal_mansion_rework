using RoyalMasion.Code.Infrastructure.Data;

namespace RoyalMansion.Code.UnityLogic.NPC.NpcBehaviour
{
    public interface INpcBehaviour
    {
        NpcState State { get; }

        void Enter();
        void Exit();
    }
}