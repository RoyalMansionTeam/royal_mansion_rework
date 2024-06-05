using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.ContextObjects.MansionContext
{
    public class MansionSpawnPointData : MonoBehaviour
    {
        [SerializeField] private Transform _guestSpawnPoint;

        public Transform GuestSpawnPoint => _guestSpawnPoint;
    }

}
