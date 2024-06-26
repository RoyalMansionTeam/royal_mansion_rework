using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RoyalMasion.Code.Infrastructure.Services.UIFactory
{
    public class UIFactory : IUIFactory
    {
        private Transform _uiRoot;
        private readonly IStaticDataService _staticDataService;
        private readonly IObjectResolver _objectResolver;

        [Inject]
        public UIFactory(IStaticDataService staticDataService, IObjectResolver objectResolver)
        {
            _staticDataService = staticDataService;
            _objectResolver = objectResolver;
        }

        public void CreateUiRoot()
        {
            if (_uiRoot != null)
                Object.Destroy(_uiRoot.gameObject);
            WindowConfig config = _staticDataService.GetWindow(WindowID.UiRoot);
            GameObject uiRoot = _objectResolver.Instantiate(config.Window);
            _uiRoot = uiRoot.transform;
        }

        public GameObject CreateWindow(in WindowID windowID, bool unique = false)
        {
            WindowConfig config = _staticDataService.GetWindow(windowID);
            GameObject window;
            if (unique & _uiRoot.Find(config.Window.name + "(Clone)") != null)
                window = _uiRoot.Find(config.Window.name + "(Clone)").gameObject;
            else
                window = _objectResolver.Instantiate(config.Window, _uiRoot);
            return window;
        }

        public void ClearUIRoot()
        {
            if (_uiRoot == null)
                return;
            for (int i = 0; i < _uiRoot.childCount; i++)
                Object.Destroy(_uiRoot.GetChild(i).gameObject);
        }
    }
}