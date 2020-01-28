using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Control = PixelWizards.PhysicalMaterialManager.PhysicalMaterialController;
using Loc = PixelWizards.PhysicalMaterialManager.PhysicalMaterialLoc;                                 // string localization table

namespace PixelWizards.PhysicalMaterialManager
{

    [CustomEditor(typeof(PhysicalMaterialLibrary))]
    public class PhysicalMaterialLibraryEditor : Editor
    {
        private PhysicalMaterialLibrary libraryConfig;

        private void OnEnable()
        {
            libraryConfig = (PhysicalMaterialLibrary)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(10f);

                GUILayout.Label(Loc.WINDOW_HEADER, EditorStyles.boldLabel);
                GUILayout.Label(Loc.HELP_HEADER, EditorStyles.helpBox);

                GUILayout.Space(10f);

                if ( GUILayout.Button(Loc.LABEL_SHOWMATERIALLIBRARY, GUILayout.Height(35f)))
                {
                    PhysicalMaterialEditor.ShowWindow(libraryConfig);
                }
            }
            GUILayout.EndVertical();

        }
    }
}
