namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class GameResourceSaveData
    {
        public int ResourceType;
        public int Amount;

        public GameResourceSaveData(int resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }
}
