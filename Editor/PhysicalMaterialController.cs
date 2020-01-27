#define DEBUG_LOGGING
using UnityEditor;
using UnityEngine;

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

        public static void CreateNewMaterialLibrary()
        {
            var name = EditorUtility.SaveFilePanelInProject("Create New Physical Material Library", "PhysicalMaterialLibrary.asset", "asset", "Please enter a file name", "Assets");
            if (name.Length != 0)
            {
                // create new scriptable object for the library, save it and then refresh
                library = ScriptableObject.CreateInstance<PhysicalMaterialLibrary>();

                AssetDatabase.CreateAsset(library, name);
                AssetDatabase.SaveAssets();
               // AssetDatabase.Refresh();
#if DEBUG_LOGGING
                Debug.Log("Created Physical Library: " + name);
#endif
            }
        }

        public static void AddNewPhysicalMaterial()
        {
            var newMat = new PhysicalMaterialEntry
            {
                physicMaterial = CreatePhysicalMaterial()           // create the physicmaterial on disk
            };
            newMat.name = newMat.physicMaterial.name;
            library.entries.Add(newMat);
            AssetDatabase.SaveAssets();
         //   AssetDatabase.Refresh();
#if DEBUG_LOGGING
            Debug.Log("Added new material to library");
#endif
        }

        public static PhysicMaterial CreatePhysicalMaterial()
        {
            var mat = new PhysicMaterial();

            // prompt user where to save the file
            string name = EditorUtility.SaveFilePanelInProject("Save Physical Material", "physicmat.asset", "asset", "Please enter a file name", "Assets/GameData/Audio");

            AssetDatabase.CreateAsset(mat, name);
            AssetDatabase.SaveAssets();
         //   AssetDatabase.Refresh();
            return mat;
        }

        public static void DeleteMaterialFromLibrary( PhysicalMaterialEntry entry)
        {
            if( entry != null)
            {
                if( library.entries.Contains(entry))
                {
                    library.entries.Remove(entry);
                }
            }
        }
    }
}