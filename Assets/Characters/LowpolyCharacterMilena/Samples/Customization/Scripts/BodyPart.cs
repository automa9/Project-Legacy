using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.Demos.LowpolyCustomization
{
    public enum BodyPartType
    {
        Head = 0,
        Neck_A,
        Neck_B,
        Chest,
        Shoulders,
        Breasts,
        Midriff,
        Waist,
        Arms_Upper,
        Arms_Lower,
        Hands,
        Fingers,
        Hips,
        Briefs,
        Legs_Butt,
        Legs_Upper,
        Legs_Upper_001,
        Legs_Knee,
        Legs_Lower,
        Feet
    }

    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class BodyPart : MonoBehaviour
    {
        public BodyPartType type;

        public SkinnedMeshRenderer Renderer => GetComponent<SkinnedMeshRenderer>();
    }
}
