using RoyalMasion.Code.Extensions;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class NpcSaveData : MansionSaveableCategory
    {
        public Vector3Data Position;
        public string AssignedUnitID;
        public int State;
        public Vector3Data TargetPosition;
        public string TargetUnitID;

        public NpcSaveData(string uniqueSaveID, Vector3Data position, string assignedUnitID, int state, Vector3Data targetPosition, string targetUnitID)
        {
            UniqueSaveID = uniqueSaveID;
            Position = position;
            AssignedUnitID = assignedUnitID;
            State = state;
            TargetPosition = targetPosition;
            TargetUnitID = targetUnitID;
        }

    }
}