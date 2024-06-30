using System;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.CameraLogic
{
    public class DraggableCamera : MonoBehaviour
    {

        [SerializeField] private Bounds _draggableArea;

        public float dragSpeed = .1f;

        private Vector3 dragOrigin, _cameraPosition;
        private bool _enableDrag;


        private void Update()
        {
            if (!_enableDrag)
                return;
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
            else if (Input.GetTouch(0).phase != TouchPhase.Moved || Input.touchCount != 1)
                return;
            Vector3 inputPosition = Input.GetTouch(0).position;
#endif
            Drag(inputPosition);
        }


        public void SetDragActive(bool state) =>
            _enableDrag = state;

        private void Drag(Vector3 inputPosition)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(inputPosition - dragOrigin);
            _cameraPosition = new Vector3((-1) * pos.x * dragSpeed, 0, (-1) * pos.y * dragSpeed);
            Vector3 newWorldPoint = Camera.main.ScreenToWorldPoint(_cameraPosition);
            _cameraPosition = _draggableArea.Contains(newWorldPoint) ?
                _cameraPosition : Vector3.zero;
            transform.Translate(_cameraPosition, Space.World);
        }
        private void Zoom()
        {

        }
    }
}
