using UnityEditor;
using UnityEngine;
using Control = PixelWizards.PhysicalMaterialManager.PhysicalMaterialController;
using Loc = PixelWizards.PhysicalMaterialManager.PhysicalMaterialLoc;                                 // string localization table

namespace PixelWizards.PhysicalMaterialManager
{
    /// <summary>
    /// strings db
    /// </summary>
    public static class PhysicalMaterialLoc
    {
        public const string MENUITEMPATH = "Window/Audio/Physical Material Editor";
        public const string WINDOWTITLE = "Physical Material Editor";
        public const string PROJECTROOT = "Prefab Root:";

        public const string BUTTON_ADDNEWMAT = "Add New Library Material";
        public const string BUTTON_CREATEPHYSICALMATERIAL = "Create New";
        public const string BUTTON_REFRESHLIBRARY = "Refresh Library";
        public const string BUTTON_DELETEMAT = "Delete";
        public const string BUTTON_SAVELIBRARY = "Save Changes";
        public const string BUTTON_APPLY = "Apply";

        public const string LABEL_GRAVITY = "Gravity";

        public const string HEADER_INDEX = "Idx";
        public const string HEADER_LABEL = "Label";
        public const string HEADER_PHYSICMATERIAL = "Physics Material";
        public const string HEADER_STATIC = "Static Friction";
        public const string HEADER_DYNAMIC = "Dynamic Friction";
        public const string HEADER_BOUNCINESS = "Bounciness";
        public const string HEADER_BOUNCECOMBINE = "Bounce Combine";
        public const string HEADER_FRICTIONCOMBINE = "Friction Combine";
        public const string HEADER_ACTIONS = "Actions";

        public const string WINDOW_HEADER = "Manage Physical Material Library";
        public const string HELP_HEADER = "The Physical Material Library provides a centralized dashboard for editing and managing PhysicMaterial properties for your project.";

        public const string LABEL_LIBRARY = "Material Library:";
        public const string LABEL_SHOWMATERIALLIBRARY = "Open Material Library Editor"; // displayed on the scriptable object

        public const string DIALOG_CREATENEWLIBRARY = "Create New Physical Material Library";
        public const string DIALOG_SAVEPHYSICALMATERIAL = "Save Physical Material";
        public const string DIALOG_ENTERFILENAME = "Please enter a file name";
    }

    /// <summary>
    /// Central dashboard for editing / managing the PhysicMaterials in your project
    /// </summary>
    public class PhysicalMaterialEditor : EditorWindow
    {
        private static Vector2 minWindowSize = new Vector2(1050, 510);
        private static Vector2 scrollPosition = Vector2.zero;

        /// <summary>
        /// Opens the PhysicalMaterial GUI 
        /// </summary>
        [MenuItem(Loc.MENUITEMPATH)]
        private static void ShowWindow()
        {
            var thisWindow = GetWindow<PhysicalMaterialEditor>(false, Loc.WINDOWTITLE, true);
            thisWindow.minSize = minWindowSize;
            thisWindow.Reset();
        }

        private void OnEnable()
        {
            Reset();
        }

        private void Reset()
        {
            Control.Init();
        }

        private void RenderColumnHeaders()
        {
            GUILayout.BeginHorizontal();
            {
                //GUILayout.Label(Loc.HEADER_INDEX, EditorStyles.boldLabel, GUILayout.Width(30f));
                //GUILayout.Space(5f);
                GUILayout.Space(10f);
                GUILayout.Label(Loc.HEADER_ACTIONS, EditorStyles.boldLabel, GUILayout.Width(50f));
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_PHYSICMATERIAL, EditorStyles.boldLabel, GUILayout.Width(150f));
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_STATIC, EditorStyles.boldLabel, GUILayout.Width(150f));
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_DYNAMIC, EditorStyles.boldLabel, GUILayout.Width(150f));
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_BOUNCINESS, EditorStyles.boldLabel, GUILayout.Width(150f));
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_BOUNCECOMBINE, EditorStyles.boldLabel, GUILayout.Width(120f));
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_FRICTIONCOMBINE, EditorStyles.boldLabel, GUILayout.Width(120f));
                
            }
            GUILayout.EndHorizontal();
        }
        
        private void OnGUI()
        {
            GUILayout.Space(10f);
            GUILayout.BeginVertical();
            {
                GUILayout.Space(10f);

                GUILayout.Label(Loc.WINDOW_HEADER, EditorStyles.boldLabel);
                GUILayout.Label(Loc.HELP_HEADER, EditorStyles.helpBox);

                GUILayout.Space(10f);

                if (Control.Library != null)
                {
                    GUILayout.Space(10f);

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(10f);

                        GUILayout.Label(Loc.LABEL_GRAVITY, GUILayout.Width(150f));
                        Control.Library.gravity = EditorGUILayout.Vector3Field(GUIContent.none, Control.Library.gravity);       // read in the gravity desired
                        Physics.gravity = Control.Library.gravity;          // update global gravity
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10f);

                    if( Control.Library.entries.Count > 0)
                    {
                        RenderColumnHeaders();
                    }
                        
                    GUILayout.Space(5f);

                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.ExpandHeight(true));
                    {
                        EditorGUI.BeginChangeCheck();
                        for (var i = 0; i < Control.Library.entries.Count; i++)
                        {
                            var entry = Control.Library.entries[i];

                            GUILayout.BeginHorizontal();
                            {
                                //GUILayout.Label(i + ":", GUILayout.Width(30f));
                                //GUILayout.Space(5f);
                                GUILayout.Space(10f);
                                if (GUILayout.Button(Loc.BUTTON_APPLY, GUILayout.Width(50f)))
                                {
                                    Control.ApplyMaterialToSelection(entry);
                                }
                                GUILayout.Space(5f);
                                entry.physicMaterial = (PhysicMaterial)EditorGUILayout.ObjectField(entry.physicMaterial, typeof(PhysicMaterial), false, GUILayout.Width(150f));

                                GUILayout.Space(5f);
                                entry.dynamicFriction = EditorGUILayout.Slider(entry.dynamicFriction, 0, 1, GUILayout.Width(150f));
                                entry.physicMaterial.dynamicFriction = entry.dynamicFriction;

                                GUILayout.Space(5f);
                                entry.staticFriction = EditorGUILayout.Slider(entry.staticFriction, 0, 1, GUILayout.Width(150f));
                                entry.physicMaterial.staticFriction = entry.staticFriction;

                                GUILayout.Space(5f);
                                entry.bounciness = EditorGUILayout.Slider(entry.bounciness, 0, 1, GUILayout.Width(150f));
                                entry.physicMaterial.bounciness = entry.bounciness;

                                GUILayout.Space(5f);
                                entry.frictionCombine = (PhysicMaterialCombine)EditorGUILayout.EnumPopup(entry.frictionCombine, GUILayout.Width(120f));
                                entry.physicMaterial.frictionCombine = entry.frictionCombine;

                                GUILayout.Space(5f);
                                entry.bounceCombine = (PhysicMaterialCombine)EditorGUILayout.EnumPopup(entry.bounceCombine, GUILayout.Width(120f));
                                entry.physicMaterial.bounceCombine = entry.bounceCombine;

                                GUILayout.Space(5f);

                                //if (GUILayout.Button(Loc.BUTTON_DELETEMAT, GUILayout.Width(50f)))
                                //{
                                //    Control.DeleteMaterialFromLibrary(entry);
                                //}
                               
                            }
                            GUILayout.EndHorizontal();
                        }
                        EditorGUI.EndChangeCheck();
                    }
                    GUILayout.EndScrollView();

                    GUILayout.Space(10f);
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(Loc.BUTTON_ADDNEWMAT, GUILayout.Width(250f), GUILayout.Height(35f)))
                        {
                            Control.AddNewPhysicalMaterial();
                        }

                        if( GUILayout.Button(Loc.BUTTON_REFRESHLIBRARY, GUILayout.Width(250f), GUILayout.Height(35f)))
                        {
                            Control.RefreshLibrary();
                        }
 
                        if (GUILayout.Button(Loc.BUTTON_SAVELIBRARY, GUILayout.Width(250f), GUILayout.Height(35f)))
                        {
                            Control.SaveMaterialLibrary();
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(10f);
        }
    }
}