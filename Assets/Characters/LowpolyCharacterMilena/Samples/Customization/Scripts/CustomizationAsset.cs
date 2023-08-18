using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.Demos.LowpolyCustomization
{
    public enum CustomizationSlot
    {
        Hair,
        Accessory_Neck,
        UpperBody,
        Gloves,
        LowerBody,
        Shoes,
    }

    [CreateAssetMenu(fileName = "CustomizationAsset", menuName = "Rukha93/Create CustomizationAsset", order = 1)]
    public class CustomizationAsset : ScriptableObject
    {
        [SerializeField] private CustomizationSlot m_Slot;
        [SerializeField] private Mesh[] m_Meshes;
        [SerializeField] private BodyPartType[] m_HiddenParts;

        public CustomizationSlot Slot => m_Slot;
        public Mesh[] Meshes => m_Meshes;
        public BodyPartType[] HiddenParts => m_HiddenParts;
    }
}
