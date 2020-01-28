using UnityEditor;
using UnityEngine;
using Loc = PixelWizards.PhysicalMaterialManager.PhysicalMaterialLoc;                                 // string localization table

namespace PixelWizards.PhysicalMaterialManager
{

    public static class PhysicalMaterialController
    {
        public static PhysicalMaterialLibrary library;
        public static bool initialized = false;

        public static void Init()
        {
            if (initialized)
                return;

            initialized = true;
        }

        public static void SaveMaterialLibrary()
        {
            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void CreateNewMaterialLibrary()
        {
            var name = EditorUtility.SaveFilePanelInProject(Loc.DIALOG_CREATENEWLIBRARY, "PhysicalMaterialLibrary.asset", "asset", Loc.DIALOG_ENTERFILENAME, "Assets");
            if (name.Length != 0)
            {
                // create new scriptable object for the library, save it and then refresh
                library = ScriptableObject.CreateInstance<PhysicalMaterialLibrary>();

                AssetDatabase.CreateAsset(library, name);
                SaveMaterialLibrary();
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
                newMat.physicMaterial = physMat;           // create the physicmaterial on disk
                newMat.name = newMat.physicMaterial.name;
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