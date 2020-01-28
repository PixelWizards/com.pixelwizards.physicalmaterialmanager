using UnityEditor;
using UnityEngine;
using Loc = PixelWizards.PhysicalMaterialManager.PhysicalMaterialLoc;                                 // string localization table

namespace PixelWizards.PhysicalMaterialManager
{

    public static class PhysicalMaterialController
    {
        public static PhysicalMaterialLibrary library = new PhysicalMaterialLibrary();
        public static bool initialized = false;

        public static void Init()
        {
            if (initialized)
                return;

            RefreshLibrary();

            initialized = true;
        }

        public static void SaveMaterialLibrary()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RefreshLibrary()
        {
            library.entries.Clear();

            string[] guids;

            // search for a ScriptObject called ScriptObj
            guids = AssetDatabase.FindAssets("t:PhysicMaterial");
            foreach (string guid in guids)
            {
                var matpath = AssetDatabase.GUIDToAssetPath(guid);
                var physMat = (PhysicMaterial) AssetDatabase.LoadAssetAtPath(matpath, typeof(PhysicMaterial));
                var matEntry = new PhysicalMaterialEntry()
                {
                    physicMaterial = physMat,
                    dynamicFriction = physMat.dynamicFriction,
                    staticFriction = physMat.staticFriction,
                    bounciness = physMat.bounciness,
                    bounceCombine = physMat.bounceCombine,
                    frictionCombine = physMat.frictionCombine,
                };
               
                library.entries.Add(matEntry);
            }
        }

        public static void AddNewPhysicalMaterial()
        {
            // prompt user where to save the file
            string name = EditorUtility.SaveFilePanelInProject(Loc.DIALOG_SAVEPHYSICALMATERIAL, "physicmat.asset", "asset", Loc.DIALOG_ENTERFILENAME, "Assets/GameData/Audio");
            if(name.Length != 0)
            {
                var newMat = new PhysicalMaterialEntry();
                var physMat = new PhysicMaterial();
                AssetDatabase.CreateAsset(physMat, name);
                newMat.physicMaterial = physMat;
                library.entries.Add(newMat);
                SaveMaterialLibrary();
            }
        }

        public static void DeleteMaterialFromLibrary( PhysicalMaterialEntry entry)
        {
            if( entry != null)
            {
                if( library.entries.Contains(entry))
                {
                    library.entries.Remove(entry);
                    SaveMaterialLibrary();
                }
            }

        }
    }
}