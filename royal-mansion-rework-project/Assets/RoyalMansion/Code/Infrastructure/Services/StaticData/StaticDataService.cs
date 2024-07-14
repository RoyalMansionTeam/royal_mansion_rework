using System.Collections.Generic;
using System.Threading.Tasks;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.StaticData;
using Unity.VisualScripting;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Services.StaticData
{
    /// <summary>
    /// Provide static data (balance or other GD parameters) for services and other entities
    /// </summary>
    public class StaticDataService : IStaticDataService
    {
        private readonly Dictionary<WindowID, WindowConfig> _windows = new();

        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public GameStaticData GameData { get; private set; }

        public async Task Load()
        {
            GameData = await _assetProvider.Load<GameStaticData>(AssetAddress.GAME_STATIC_DATA_PATH);
            LoadWindows();
        }

        public WindowConfig GetWindow(WindowID id)
        {
            return _windows.TryGetValue(id, out var windowConfig) ? windowConfig : null;
        }
        private void LoadWindows()
        {
            foreach (var window in GameData.Windows)
                _windows.Add(window.ID, window);
        }
    }
}