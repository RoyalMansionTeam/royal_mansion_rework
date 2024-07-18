using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMasion.Code.Infrastructure.Data;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Services.UIFactory
{
    public interface IUIFactory
    {
        Canvas UICanvas { get; }

        void CreateUiRoot();
        GameObject CreateWindow(in WindowID windowID, bool unique = false);
        MansionUnitUIHandler CreateUnitUIHandler();
        void ClearUIRoot();
    }
}