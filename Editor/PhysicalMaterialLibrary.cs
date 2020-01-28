using System.Collections.Generic;
using UnityEngine;

namespace PixelWizards.PhysicalMaterialManager
{
    [System.Serializable]
    public class PhysicalMaterialEntry
    {
        public PhysicMaterial physicMaterial;
        public float dynamicFriction;
        public float staticFriction;
        public float bounciness;
        public PhysicMaterialCombine frictionCombine;
        public PhysicMaterialCombine bounceCombine;
    }

    [System.Serializable]
    public class PhysicalMaterialLibrary
    {
        public Vector3 gravity = new Vector3(0, -9.8f, 0);
        public List<PhysicalMaterialEntry> entries = new List<PhysicalMaterialEntry>();
    }
}