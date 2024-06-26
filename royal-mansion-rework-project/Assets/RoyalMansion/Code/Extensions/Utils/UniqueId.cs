using System;
using UnityEngine;

namespace RoyalMasion.Code.Extensions.Utils
{
    public class UniqueId : MonoBehaviour
    {
        public string GenerateId() =>
            $"{gameObject.scene.name}_{Guid.NewGuid()}";
    }
}