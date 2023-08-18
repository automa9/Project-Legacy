using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.Utilities
{
    public class LookControl : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        [SerializeField] private SkinnedMeshRenderer m_Face;

        [Space]
        [SerializeField] private int m_LeftBlendShapeIndex = 3;
        [SerializeField] private int m_RightBlendShapeIndex = 4;
        [SerializeField] private int m_UpBlendShapeIndex = 1;
        [SerializeField] private int m_DownBlendShapeIndex = 2;

        [Space]
        public Transform LookTarget;
        public Vector3 HeadToEyeOffset = new Vector3(0, 0.085f, 0.09f);

        [Range(0, 90)]
        public float EyeHorizontalMaxRange = 60;
        [Range(0, 90)]
        public float EyeVerticalMaxRange = 60;

        [Range(0, 1f)]
        public float HeadRotationWeight = 0.1f;
        [Range(0, 1)]
        public float NeckRotationWeight = 0.25f;

        [Space]
        [Range(0, 1)]
        public float HeadBlendWeight = 1;
        [Range(0, 1)]
        public float EyeBlendWeight = 1;

        [Space]
        public float LerpSpeed = 10;

        private Transform m_ChestBone;
        private Transform m_NeckBone;
        private Transform m_HeadBone;

        private Quaternion m_CurrentNeckRotation;
        private Quaternion m_CurrentHeadRotation;

        private Quaternion m_TargetNeckRotation;
        private Quaternion m_TargetHeadRotation;

        private void OnValidate()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();

            if (m_Animator == null)
            {
                this.enabled = false;
                Debug.LogError("missing animator");
            }
        }

        void OnEnable()
        {
            m_ChestBone = m_Animator.GetBoneTransform(HumanBodyBones.UpperChest) ?? m_Animator.GetBoneTransform(HumanBodyBones.Chest);
            m_NeckBone = m_Animator.GetBoneTransform(HumanBodyBones.Neck);
            m_HeadBone = m_Animator.GetBoneTransform(HumanBodyBones.Head);

            m_CurrentNeckRotation = m_NeckBone.rotation;
            m_CurrentHeadRotation = m_HeadBone.rotation;
        }

        private void OnDisable()
        {
            m_NeckBone.localRotation = Quaternion.identity;
            m_HeadBone.localRotation = Quaternion.identity;

            m_Face.SetBlendShapeWeight(m_UpBlendShapeIndex, 0);
            m_Face.SetBlendShapeWeight(m_DownBlendShapeIndex, 0);
            m_Face.SetBlendShapeWeight(m_RightBlendShapeIndex, 0);
            m_Face.SetBlendShapeWeight(m_LeftBlendShapeIndex, 0);
        }

        public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis)
        {
            Vector3 right = Vector3.Cross(axis, forward).normalized;
            forward = Vector3.Cross(right, axis).normalized;
            return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
        }


        private void OnDrawGizmosSelected()
        {
            if (m_HeadBone == null)
                return;
            if (LookTarget == null)
                return;

            Vector3 origin = m_HeadBone.TransformPoint(HeadToEyeOffset);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, LookTarget.position);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(origin, origin + m_HeadBone.forward * 0.1f);
        }

        private void LateUpdate()
        {
            //first animate the head
            Vector3 origin = m_HeadBone.TransformPoint(HeadToEyeOffset);
            Vector3 lookVector = (LookTarget.position - origin).normalized;
            Vector3 chestForward = m_ChestBone.forward;
            float dot = Vector3.Dot(chestForward, lookVector) * 1.2f; // limit the amount the head and neck can be turned away from the chest

            m_TargetNeckRotation = Quaternion.Lerp(
                m_NeckBone.rotation,
                Quaternion.LookRotation(lookVector, transform.up),
                NeckRotationWeight * NeckRotationWeight * Mathf.Clamp(dot, 0.4f, 1));

            m_TargetHeadRotation = Quaternion.Lerp(
                m_HeadBone.rotation,
                Quaternion.LookRotation(lookVector, transform.up),
                HeadRotationWeight * HeadBlendWeight * Mathf.Clamp(dot, 0.2f, 1));

            //smoothly rotate from previous to next rotation
            m_CurrentNeckRotation = m_NeckBone.rotation = Quaternion.Lerp(m_CurrentNeckRotation, m_TargetNeckRotation, Time.deltaTime * LerpSpeed);
            m_CurrentHeadRotation = m_HeadBone.rotation = Quaternion.Lerp(m_CurrentHeadRotation, m_TargetHeadRotation, Time.deltaTime * LerpSpeed);

            //then animate the eyets
            Vector3 headForward = m_HeadBone.forward;
            origin = m_HeadBone.TransformPoint(HeadToEyeOffset);
            lookVector = (LookTarget.position - origin).normalized;
            dot = Vector3.Dot(headForward, lookVector);

            if (dot > 0)
            {
                float vertical = AngleOffAroundAxis(lookVector, headForward, -m_HeadBone.right);
                m_Face.SetBlendShapeWeight(m_UpBlendShapeIndex, Mathf.Clamp(vertical / EyeVerticalMaxRange, 0f, 1f) * 100 * EyeBlendWeight);
                m_Face.SetBlendShapeWeight(m_DownBlendShapeIndex, Mathf.Clamp(vertical / EyeVerticalMaxRange, -1f, 0f) * -100 * EyeBlendWeight);

                float horizontal = AngleOffAroundAxis(lookVector, headForward, m_HeadBone.up);
                m_Face.SetBlendShapeWeight(m_RightBlendShapeIndex, Mathf.Clamp(horizontal / EyeHorizontalMaxRange, 0f, 1f) * 100 * EyeBlendWeight);
                m_Face.SetBlendShapeWeight(m_LeftBlendShapeIndex, Mathf.Clamp(horizontal / EyeHorizontalMaxRange, -1f, 0f) * -100 * EyeBlendWeight);
            }
            else
            {
                m_Face.SetBlendShapeWeight(m_UpBlendShapeIndex, 0);
                m_Face.SetBlendShapeWeight(m_DownBlendShapeIndex, 0);
                m_Face.SetBlendShapeWeight(m_RightBlendShapeIndex, 0);
                m_Face.SetBlendShapeWeight(m_LeftBlendShapeIndex, 0);
            }
        }
    }
}