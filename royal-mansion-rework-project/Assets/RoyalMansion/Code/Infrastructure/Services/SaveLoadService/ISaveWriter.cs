namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public interface ISaveWriter : ISaveable
    {
        void SaveProgress(GameProgress progress);
    }
}