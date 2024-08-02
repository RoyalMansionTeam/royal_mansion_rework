using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    [CreateAssetMenu(fileName = "NPC_prefabs_data", menuName = "Prefabs/NPC prefabs")]
    public class NpcPrefabData : ScriptableObject
    {
        [SerializeField] private List<GameObject> _guestPrefabs;
        [SerializeField] private List<GameObject> _waiterPrefabs;
        [SerializeField] private List<GameObject> _cookPrefabs;
        [SerializeField] private List<GameObject> _maidPrefabs;

        private Dictionary<Type, List<GameObject>> _prefabs;

        public Dictionary<Type, List<GameObject>> Prefabs => _prefabs;

        private void OnValidate()
        {
            _prefabs = new()
            {
                [typeof(GuestNPC)] = _guestPrefabs,
                [typeof(WaiterNPC)] = _waiterPrefabs,
                [typeof(CookNPC)] = _cookPrefabs,
                [typeof(MaidNPC)] = _maidPrefabs,
            };
        }

    }
}


