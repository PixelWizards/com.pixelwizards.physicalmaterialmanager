using UnityEditor;
using UnityEngine;
using Loc = PixelWizards.PhysicalMaterialManager.PhysicalMaterialLoc;                                 // string localization table

namespace PixelWizards.PhysicalMaterialManager
{
    /// <summary>
    /// PhysicMaterial library controller
    /// </summary>
    public static class PhysicalMaterialController
    {
        private static PhysicalMaterialLibrary library = new PhysicalMaterialLibrary();
        private static bool initialized = false;

        /// <summary>
        /// The PhysicMaterial library
        /// </summary>
        public static PhysicalMaterialLibrary Library { get => library; set => library = value; }

        /// <summary>
        /// Entry point, initialization of the controller
        /// </summary>
        public static void Init()
        {
            if (initialized)
                return;

            RefreshLibrary();

            initialized = true;
        }

        /// <summary>
        /// saves any dirties objects and refreshes the asset db
        /// </summary>
        public static void SaveMaterialLibrary()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// refreshes the physicmaterial library from the project
        /// </summary>
        public static void RefreshLibrary()
        {
            Library.entries.Clear();

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
               
                Library.entries.Add(matEntry);
            }
        }

        /// <summary>
        /// Creates a new PhysicMaterial - prompts for save location and adds it to the list
        /// </summary>
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
                Library.entries.Add(newMat);
                SaveMaterialLibrary();
            }
        }

        /// <summary>
        /// removes the selected entry from the library. note: currently does not remove the asset on disk
        /// </summary>
        /// <param name="entry"></param>
        public static void DeleteMaterialFromLibrary( PhysicalMaterialEntry entry)
        {
            if( entry != null)
            {
                if( Library.entries.Contains(entry))
                {
                    Library.entries.Remove(entry);
                    SaveMaterialLibrary();
                }
            }

        }
    }
}