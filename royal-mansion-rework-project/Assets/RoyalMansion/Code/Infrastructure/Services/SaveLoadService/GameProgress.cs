using RoyalMasion.Code.Infrastructure.Saving;
namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public class GameProgress
    {
        public PlayerSettings PlayerSettings;
        public MansionProgress MansionProgress;
        public IngameDataProgress IngameDataProgress;

        public GameProgress()
        {
            PlayerSettings = new();
            MansionProgress = new();
            IngameDataProgress = new();
        }
    }
}