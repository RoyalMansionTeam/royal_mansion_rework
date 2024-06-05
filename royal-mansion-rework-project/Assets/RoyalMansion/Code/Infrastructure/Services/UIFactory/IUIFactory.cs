using RoyalMasion.Code.Infrastructure.Data;
using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.Services.UIFactory
{
    public interface IUIFactory
    {
        void CreateUiRoot();
        GameObject CreateWindow(in WindowID windowID, bool unique = false);
        void ClearUIRoot();
    }
}