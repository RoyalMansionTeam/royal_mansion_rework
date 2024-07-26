using UnityEngine;
using UnityEngine.UI;

namespace RoyalMasion.Code.UI.Windows
{
    public class CollectablePopUpWindow : WindowBase
    {
        [SerializeField] private Image _valueIcon;

        private WindowBase _parentObject;
        public void SetIcon(Sprite targetSprite) =>
            _valueIcon.sprite = targetSprite;
        public void SetParentObject(WindowBase parent) =>
            _parentObject = parent;
        private void OnDestroy()
        {
            _parentObject.CloseWindow();
        }
    }
}
