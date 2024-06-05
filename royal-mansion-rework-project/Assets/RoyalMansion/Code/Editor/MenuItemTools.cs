using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RoyalMasion.Code.Editor
{
    public class MenuItemTools : EditorWindow
    {
        [MenuItem("Shortcuts/Open Bootstrap Scene", false, 1)]
        public static void OpenBootstrapScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/BootstrapScene.unity", OpenSceneMode.Single);
        }
    }

}
