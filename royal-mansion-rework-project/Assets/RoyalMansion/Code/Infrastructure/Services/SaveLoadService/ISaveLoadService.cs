namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        void ApplyProgress();
        GameProgress LoadProgress();
    }
}