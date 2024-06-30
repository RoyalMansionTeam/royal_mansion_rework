using System;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.CameraLogic
{
    public class DraggableCamera : MonoBehaviour
    {
        [System.Serializable]
        protected class DragableCameraInternalSettings
        {
            public Bounds area;
        }
        [SerializeField] private DragableCameraInternalSettings _dragableCameraInternalSettings;
        public float dragSpeed = .1f;
        private Vector3 dragOrigin;
        /*private Plane _plane;
        private Ray _ray;
        private Vector2 _touchPosition;
        private bool _isInDrag = false;*/
        private Vector3 _pointPosition, _cameraPosition, _lastPosition, _currentPosition,
            _delta, _worldPosition; 

        private void Start()
        {
            //_plane = new Plane(Vector2.up, 0);
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }
            if (!Input.GetMouseButton(0))
                return;
            Vector3 inputPosition = Input.mousePosition;
#else
            if (Input.touchCount == 1 & Input.GetTouch(0).phase == TouchPhase.Began)
                dragOrigin = Input.GetTouch(0).position;
            else
                return;
#endif
            Drag(inputPosition);
        }

        private void Drag(Vector3 inputPosition)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(inputPosition - dragOrigin);
            _cameraPosition = new Vector3((-1) * pos.x * dragSpeed, 0, (-1) * pos.y * dragSpeed);
            Vector3 newWorldPoint = Camera.main.ScreenToWorldPoint(_cameraPosition);
            _cameraPosition = _dragableCameraInternalSettings.area.Contains(newWorldPoint) ?
                _cameraPosition : Vector3.zero;
            transform.Translate(_cameraPosition, Space.World);
        }
    }
}
