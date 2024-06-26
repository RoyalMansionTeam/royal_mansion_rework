namespace RoyalMasion.Code.Infrastructure.Services.SaveLoadService
{
    public interface IPersistentProgressService
    {
        GameProgress Progress { get; set; }
    }
}