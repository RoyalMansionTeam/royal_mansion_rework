using RoyalMasion.Code.Infrastructure.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.CameraLogic
{
    public class UnitVirtualCamerasData : MonoBehaviour
    {
        [SerializeField] private List<UnitVirtualCamera> _virtialCameras;
        public List<UnitVirtualCamera> VirtialCameras => _virtialCameras;
    }

    [System.Serializable]
    public class UnitVirtualCamera
    {
        public Cinemachine.CinemachineVirtualCamera VirtualCamera;
        public LayerMask IgnoredLayers;
        public ApartmentAreaType ApartmentAreaType;
    }
}
