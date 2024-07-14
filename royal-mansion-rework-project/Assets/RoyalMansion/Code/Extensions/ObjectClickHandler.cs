using System;
using UnityEngine;

namespace RoyalMasion.Code.Editor
{
    public class ObjectClickHandler : MonoBehaviour
    {
        public Action ClickHandled;
        private Vector2 _screenPosition;
        private Ray _ray;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 mousePos = Input.mousePosition;
                _screenPosition = new Vector2(mousePos.x, mousePos.y);
            }
            else
                return;
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    _screenPosition = Input.GetTouch(0).position;
            }
            else
                return;
#endif
            var screenToCameraDistance = Camera.main.nearClipPlane;
            _ray = Camera.main.ScreenPointToRay(_screenPosition);

            if (!Physics.Raycast(_ray.origin, _ray.direction * 10, out RaycastHit hit))
                return;
            if (hit.transform.gameObject == gameObject)
            {
                ClickHandled?.Invoke();
            }
        }
    }

}
