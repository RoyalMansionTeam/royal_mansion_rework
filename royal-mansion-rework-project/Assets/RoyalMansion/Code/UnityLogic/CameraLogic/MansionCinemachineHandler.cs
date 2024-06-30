using Cinemachine;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.CameraLogic
{
    public class MansionCinemachineHandler : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _mainVirtualCamera;
        [SerializeField] private Camera _mainCamera;
        private CinemachineVirtualCamera _currentVirtualCamera;

        private void Start()
        {
            _currentVirtualCamera = _mainVirtualCamera;
            _currentVirtualCamera.Priority = 1;
        }

        public void SetCameraTo(CinemachineVirtualCamera targetCam, LayerMask ignoredLayers)
        {
            targetCam.Priority += 1;
            if (_currentVirtualCamera.Priority > 0)
                _currentVirtualCamera.Priority -= 1;
            _currentVirtualCamera = targetCam;
        }

        public void ResetCamera()
        {
            _mainVirtualCamera.Priority += 1;
            if (_currentVirtualCamera.Priority > 0)
                _currentVirtualCamera.Priority -= 1;
            _currentVirtualCamera = _mainVirtualCamera;
        }
    }
}
