using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.StaticData;
using System.Threading.Tasks;

namespace RoyalMasion.Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        GameStaticData GameData { get; }
        Task Load();
        WindowConfig GetWindow(WindowID id);

    }
}