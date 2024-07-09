using RoyalMasion.Code.Infrastructure.Data;
using System;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic
{
    [Serializable]
    public class ApartmentMaterialParents
    {
        [SerializeField] public ApartmentAreaType AreaType;
        [SerializeField] public ApartmentMaterialHandler WallsHandler;
        [SerializeField] public ApartmentMaterialHandler FloorsHandler;
    }

}

