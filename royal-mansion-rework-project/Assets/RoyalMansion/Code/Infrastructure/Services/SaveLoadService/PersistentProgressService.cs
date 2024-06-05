namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public partial class SaveLoadService
    {
        public class PersistentProgressService : IPersistentProgressService
        {
            public PlayerProgress Progress { get; set; }
        }
    }
}