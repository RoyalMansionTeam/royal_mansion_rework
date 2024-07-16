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
            SetMainVirtualCam();
        }

        public void SetCameraTo(CinemachineVirtualCamera targetCam, LayerMask ignoredLayers)
        {
            targetCam.Priority += 1; //Setting virtual camera
            if (_currentVirtualCamera.Priority > 0)
                _currentVirtualCamera.Priority -= 1;
            _currentVirtualCamera = targetCam;
            if (_mainVirtualCamera.TryGetComponent(out DraggableCamera dragComponentMain)) //Turning drag option on/off for cams
                dragComponentMain.SetDragActive(false);
            if (targetCam.TryGetComponent(out DraggableCamera dragComponentTarget))
                dragComponentTarget.SetDragActive(true);
        }

        public void ResetCamera()
        {
            if (_mainVirtualCamera.TryGetComponent(out DraggableCamera dragComponentMain))
                dragComponentMain.SetDragActive(true);
            if (_currentVirtualCamera.TryGetComponent(out DraggableCamera dragComponentCurrent))
                dragComponentCurrent.SetDragActive(false);
            _mainVirtualCamera.Priority += 1;
            if (_currentVirtualCamera.Priority > 0)
                _currentVirtualCamera.Priority -= 1;
            _currentVirtualCamera = _mainVirtualCamera;
        }

        private void SetMainVirtualCam()
        {
            _currentVirtualCamera = _mainVirtualCamera;
            _currentVirtualCamera.Priority = 1;
            if (_mainVirtualCamera.TryGetComponent(out DraggableCamera dragComponent))
                dragComponent.SetDragActive(true);
        }
    }
}
