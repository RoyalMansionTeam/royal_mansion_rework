using UnityEngine;
using UnityEngine.UI;

namespace RoyalMasion.Code.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            if (CloseButton != null)
                CloseButton.onClick.AddListener(CloseWindow);
        }

        public virtual void CloseWindow()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        protected virtual void Cleanup()
        {
            if (CloseButton != null)
                CloseButton.onClick.RemoveListener(CloseWindow);
        }
    }
}
