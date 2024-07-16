using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic
{
    public class ApartmentMaterialHandler : MonoBehaviour
    {
        private Material _lastMaterial;
        private Material _tryMaterial;
        private string _lastAddressableMaterialID;


        public string AddressableMaterialID;

        private void Start()
        {
            _lastMaterial = GetComponentsInChildren<MeshRenderer>()[0].materials[0];
        }
        public void TryMaterial(Material targetMaterial, string addressableID)
        {
            _tryMaterial = targetMaterial;
            _lastAddressableMaterialID = addressableID;
            ApplyMaterial(targetMaterial);
        }

        public void CancelMaterial()
        {
            _tryMaterial = null;
            _lastAddressableMaterialID = null;
            ApplyMaterial(_lastMaterial);
        }

        public void ConfirmMaterial()
        {
            AddressableMaterialID = _lastAddressableMaterialID;
            _lastMaterial = _tryMaterial;
        }

        private void ApplyMaterial(Material targetMaterial)
        {
            foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
                meshRenderer.material = targetMaterial;
        }
    }

}

