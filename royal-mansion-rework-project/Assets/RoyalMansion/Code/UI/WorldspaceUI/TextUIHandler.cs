using UnityEngine;
using TMPro;

namespace RoyalMansion.Code.UI.WorldspaceUI
{
    public class TextUIHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textField;
        public void SetTextField(string messange) =>
            _textField.text = messange;
    }
}
