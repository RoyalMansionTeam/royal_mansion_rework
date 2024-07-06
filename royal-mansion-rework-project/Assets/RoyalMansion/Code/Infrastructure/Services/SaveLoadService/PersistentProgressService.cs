namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public partial class SaveLoadService
    {
        public class PersistentProgressService : IPersistentProgressService
        {
            public GameProgress Progress { get; set; }
        }
    }
}