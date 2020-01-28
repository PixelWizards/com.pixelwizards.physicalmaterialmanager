using UnityEditor;
using UnityEngine;
using Control = PixelWizards.PhysicalMaterialManager.PhysicalMaterialController;
using Loc = PixelWizards.PhysicalMaterialManager.PhysicalMaterialLoc;                                 // string localization table

namespace PixelWizards.PhysicalMaterialManager
{
    public static class PhysicalMaterialLoc
    {
        public const string MENUITEMPATH = "Window/Audio/Physical Material Editor";
        public const string WINDOWTITLE = "Physical Material Editor";
        public const string PROJECTROOT = "Prefab Root:";

        public const string BUTTON_ADDNEWMAT = "Add New Library Material";
        public const string BUTTON_CREATEPHYSICALMATERIAL = "Create New";
        public const string BUTTON_CREATELIBRARY = "Create Library";
        public const string BUTTON_DELETEMAT = "Delete";

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

    public class PhysicalMaterialEditor : EditorWindow
    {
        public static Vector2 minWindowSize = new Vector2(1120, 510);

        [MenuItem(Loc.MENUITEMPATH)]
        public static void ShowWindow()
        {
            var thisWindow = GetWindow<PhysicalMaterialEditor>(false, Loc.WINDOWTITLE, true);
            thisWindow.minSize = minWindowSize;
            thisWindow.Reset();
        }

        public static void ShowWindow(PhysicalMaterialLibrary thisLibrary)
        {
            if( thisLibrary != null)
            {
                Control.library = thisLibrary;
            }
            var thisWindow = GetWindow<PhysicalMaterialEditor>(false, Loc.WINDOWTITLE, true);
            thisWindow.Reset();
        }

        public void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            Control.Init();
        }

        public void RenderColumnHeaders()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(Loc.HEADER_INDEX, EditorStyles.boldLabel, GUILayout.Width(30f));
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_LABEL, EditorStyles.boldLabel, GUILayout.Width(80f));
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
                GUILayout.Space(5f);
                GUILayout.Label(Loc.HEADER_ACTIONS, EditorStyles.boldLabel, GUILayout.Width(50f));
            }
            GUILayout.EndHorizontal();
        }
        
        private void OnGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.MinWidth(minWindowSize.x), GUILayout.MinHeight(minWindowSize.y));
            {
                GUILayout.Space(10f);
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(10f);

                    GUILayout.Label(Loc.WINDOW_HEADER, EditorStyles.boldLabel);
                    GUILayout.Label(Loc.HELP_HEADER, EditorStyles.helpBox);

                    GUILayout.Space(10f);

                    GUILayout.Label(Loc.LABEL_LIBRARY, EditorStyles.boldLabel);
                    GUILayout.BeginHorizontal();
                    {
                        Control.library = (PhysicalMaterialLibrary)EditorGUILayout.ObjectField(Control.library, typeof(PhysicalMaterialLibrary), false);
                        if( Control.library == null)
                        {
                            if( GUILayout.Button(Loc.BUTTON_CREATELIBRARY))
                            {
                                Control.CreateNewMaterialLibrary();
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (Control.library != null)
                    {
                        GUILayout.Space(10f);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(10f);

                            GUILayout.Label(Loc.LABEL_GRAVITY, GUILayout.Width(150f));
                            Control.library.gravity = EditorGUILayout.Vector3Field(GUIContent.none, Control.library.gravity);       // read in the gravity desired
                            Physics.gravity = Control.library.gravity;          // update global gravity
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.Space(10f);

                        if( Control.library.entries.Count > 0)
                        {
                            RenderColumnHeaders();
                        }
                        
                        GUILayout.Space(5f);

                        for (var i = 0; i < Control.library.entries.Count; i++)
                        {
                            var entry = Control.library.entries[i];

                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(i + ":", GUILayout.Width(30f));
                                GUILayout.Space(5f);
                                entry.name = GUILayout.TextField(entry.name, GUILayout.Width(80f));
                                
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
                                entry.bounceCombine = (PhysicMaterialCombine)EditorGUILayout.EnumPopup(entry.bounceCombine, GUILayout.Width(120f));
                                entry.physicMaterial.bounceCombine = entry.bounceCombine;
                                GUILayout.Space(5f);
                                entry.frictionCombine = (PhysicMaterialCombine)EditorGUILayout.EnumPopup(entry.frictionCombine, GUILayout.Width(120f));
                                entry.physicMaterial.frictionCombine = entry.frictionCombine;
                                GUILayout.Space(5f);

                                if (GUILayout.Button(Loc.BUTTON_DELETEMAT, GUILayout.Width(50f)))
                                {
                                    Control.DeleteMaterialFromLibrary(entry);
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.Space(10f);
                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(Loc.BUTTON_ADDNEWMAT, GUILayout.Width(250f), GUILayout.Height(35f)))
                            {
                                Control.AddNewPhysicalMaterial();
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}