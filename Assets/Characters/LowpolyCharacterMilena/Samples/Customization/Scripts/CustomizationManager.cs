using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.Demos.LowpolyCustomization
{
    public class CustomizationManager : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer m_ReferenceMesh;

        private Dictionary<BodyPartType, List<BodyPart>> m_BodyParts;
        private Dictionary<CustomizationSlot, List<GameObject>> m_EquippedMeshes;
        private Dictionary<BodyPartType, List<GameObject>> m_HiddenMap;

        private Material m_MaterialInstance;

        public void Awake()
        {
            m_EquippedMeshes = new Dictionary<CustomizationSlot, List<GameObject>>();
            m_HiddenMap = new Dictionary<BodyPartType, List<GameObject>>();

            //get default body part renderers
            m_BodyParts = new Dictionary<BodyPartType, List<BodyPart>>();
            var parts = GetComponentsInChildren<BodyPart>();

            //use an instanced material so the original is not overriden
            m_MaterialInstance = new Material(parts[0].Renderer.sharedMaterial);

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].Renderer.sharedMaterial = m_MaterialInstance;

                if (!m_BodyParts.ContainsKey(parts[i].type))
                    m_BodyParts[parts[i].type] = new List<BodyPart>();
                m_BodyParts[parts[i].type].Add(parts[i]);
            }
        }

        public void EquipAsset(CustomizationAsset asset)
        {
            //remove equiped item if any
            Unequip(asset.Slot, false);

            if (!m_EquippedMeshes.ContainsKey(asset.Slot))
                m_EquippedMeshes[asset.Slot] = new List<GameObject>();

            //instantiate new meshes from the scriptableobject settings
            SkinnedMeshRenderer skinnedMesh;
            foreach(Mesh mesh in asset.Meshes)
            {
                var go = new GameObject(mesh.name);

                go.transform.SetParent(this.transform, false);
                m_EquippedMeshes[asset.Slot].Add(go);

                //add and setup the mesh renderer
                skinnedMesh = go.AddComponent<SkinnedMeshRenderer>();
                skinnedMesh.rootBone = m_ReferenceMesh.rootBone;
                skinnedMesh.bones = m_ReferenceMesh.bones;
                skinnedMesh.sharedMesh = mesh;
                skinnedMesh.material = m_MaterialInstance;
            }

            //map and hide default body parts
            foreach (BodyPartType part in asset.HiddenParts)
            {
                if (!m_HiddenMap.ContainsKey(part))
                    m_HiddenMap[part] = new List<GameObject>();
                m_HiddenMap[part].AddRange(m_EquippedMeshes[asset.Slot]);
            }
            UpdateBodyRenderers();
        }

        public void Unequip(CustomizationSlot slot, bool updateRenderers = true)
        {
            if (!m_EquippedMeshes.ContainsKey(slot))
                return;

            //destroy meshes
            GameObject aux;
            for (int i = 0; i < m_EquippedMeshes[slot].Count; i++)
            {
                aux = m_EquippedMeshes[slot][i].gameObject;
                aux.transform.SetParent(null);
                aux.transform.localScale = Vector3.zero;
                Destroy(aux);
            }
            m_EquippedMeshes[slot].Clear();

            //update visible body parts
            if (updateRenderers)
                UpdateBodyRenderers();
        }

        public void ToggleBodyPart(BodyPartType part, bool value)
        {
            if (!m_BodyParts.ContainsKey(part))
                return;

            foreach (var item in m_BodyParts[part])
                item.Renderer.enabled = value;
        }

        public void UpdateBodyRenderers()
        {
            foreach (var def in m_BodyParts)
            {
                if (m_HiddenMap.ContainsKey(def.Key))
                {
                    for (int i = m_HiddenMap[def.Key].Count - 1; i >= 0; i--)
                    {
                        if (m_HiddenMap[def.Key][i] != null && m_HiddenMap[def.Key][i].transform.parent != null)
                            break;
                        m_HiddenMap[def.Key].RemoveAt(i);
                    }

                    ToggleBodyPart(def.Key, m_HiddenMap[def.Key].Count == 0);
                }
                else
                {
                    ToggleBodyPart(def.Key, true);
                }
            }
        }

        public void SetColorTexture(Texture2D tex)
        {
            m_MaterialInstance.SetTexture("_MainTex", tex);
            m_MaterialInstance.SetTexture("_BaseColorMap", tex);
        }
    }
}