using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.Meta
{
    [CreateAssetMenu(fileName = "SequenceName_0", 
        menuName = "Static Data/Meta/Daily Messages/Meta Message Static Data")]
    public class MetaMessageStaticData : ScriptableObject
    {
        [TextArea(0, 5)] public string Text;
        [SerializeField] public Sprite CharacterSprite;
    }

}
