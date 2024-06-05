using System;
using UnityEngine;

namespace RoyalMasion.Code.Editor
{
  public class UniqueId : MonoBehaviour
  {
    public string Id;

    public void GenerateId() => 
      Id = $"{gameObject.scene.name}_{Guid.NewGuid()}";
  }
}