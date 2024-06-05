using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.Catalog
{
    public class DragAndDrop : MonoBehaviour
    {
        [SerializeField] private float _dragSpeed = 0.1f;
        private bool _isInDrag;
        private Vector2 _screenPosition;
        private Vector3 _worldPosition;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _isInDrag = false;
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
                _screenPosition = new Vector2(mousePos.x, mousePos.y);
            }
            else if (Input.touchCount > 0)
                _screenPosition = Input.GetTouch(0).position;
            else
                return;

            _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

            if (_isInDrag)
            {
                OnDrag();
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero);
            if (hit.collider == null)
                return;
            if (hit.transform.gameObject == gameObject)
                OnStartDrag();

        }

        private void OnStartDrag()
        {
            _isInDrag = true;
        }

        private void OnDrag()
        {
            _rb.MovePosition(Vector2.Lerp(transform.position, new Vector2(_worldPosition.x, _worldPosition.y), _dragSpeed * Time.deltaTime));
        }

        private void OnDrop()
        {
            _isInDrag = false;
        }
    }

}
