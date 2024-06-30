using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.Catalog
{
    public class DragAndDrop : MonoBehaviour
    {
        [SerializeField] private float _dragSpeed = 15f;

        Camera _targetCam;
        private Rigidbody _rb;
        private Vector3 _screenPosition;
        private Vector3 _worldPosition;
        private Plane _plane;
        private Ray _ray;
        private bool _isInDrag;
        private float _screenToCameraDistance;
        private float _rayEnter;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _targetCam = Camera.main;
            _isInDrag = false;
            _screenToCameraDistance = _targetCam.nearClipPlane;
            _plane = new Plane(Vector3.up, Vector3.zero);
        }
        private void Update()
        {
            if (_isInDrag && (Input.GetMouseButtonUp(0) ||
                (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
            {
                OnDrop();
                return;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                _screenPosition = new Vector3(mousePos.x, mousePos.y, _screenToCameraDistance);
            }
            else if (Input.touchCount > 0)
                _screenPosition = Input.GetTouch(0).position;
            else
                return;

            _ray = Camera.main.ScreenPointToRay(_screenPosition);
            if (_plane.Raycast(_ray, out _rayEnter))
                _worldPosition = _ray.GetPoint(_rayEnter);

            if (_isInDrag)
            {
                OnDrag();
                return;
            }

            if (Physics.Raycast(_ray.origin, _ray.direction * 100f, out RaycastHit hit))
            {
                if (hit.transform.gameObject == gameObject)
                    OnStartDrag();
            }

        }

        private void OnStartDrag()
        {
            _isInDrag = true;
        }

        private void OnDrag()
        {
            _rb.MovePosition(Vector3.Lerp(transform.position,
                new Vector3(_worldPosition.x, transform.position.y, _worldPosition.z), 
                _dragSpeed * Time.deltaTime));
        }

        private void OnDrop()
        {
            _isInDrag = false;
        }
    }

}
