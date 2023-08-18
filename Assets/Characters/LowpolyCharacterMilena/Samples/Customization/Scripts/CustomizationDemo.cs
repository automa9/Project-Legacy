using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Rukha93.Demos.LowpolyCustomization
{
    public class CustomizationDemo : MonoBehaviour
    {
        [SerializeField] private CustomizationManager m_Manager;
        [Space]
        [SerializeField] private CustomizationAsset[] m_AllAssets;
        [SerializeField] private Texture2D[] m_AllTextures;
        [Space]
        [SerializeField] private UICustomizationItem m_UIItemPrefab;
        [SerializeField] private UICustomizationColorItem m_ColorItemPrefab;

        [Space]
        [SerializeField] private Camera m_Camera;
        [SerializeField] private Transform m_CameraTarget_A;
        [SerializeField] private Transform m_CameraTarget_B;
        [SerializeField] private Transform m_RotateTarget;
        [SerializeField] private float m_RotateSpeed = 1;

        private float m_CameraLerp;
        private float m_CameraLerpTarget;

        private bool m_CanRotate;
        private Vector2 m_LastMousePosition;
        private float m_RotationRemaining;

        private Dictionary<CustomizationSlot, List<CustomizationAsset>> m_CustomizationOptions;
        private Dictionary<CustomizationSlot, UICustomizationItem> m_UIItemMap;
        private List<UICustomizationColorItem> m_ColorItemMap;


        private Dictionary<CustomizationSlot, int> m_IndexMap;
        private int m_ColorIndex;

        private void Start()
        {
            //initialize maps
            m_CustomizationOptions = new Dictionary<CustomizationSlot, List<CustomizationAsset>>();
            m_IndexMap = new Dictionary<CustomizationSlot, int>();
            m_UIItemMap = new Dictionary<CustomizationSlot, UICustomizationItem>();
            m_ColorItemMap = new List<UICustomizationColorItem>();

            m_AllAssets = m_AllAssets.OrderBy(a => (int)a.Slot).ToArray();

            //initialize all customization options
            for (int i = 0; i < m_AllAssets.Length; i++)
            {
                if (!m_CustomizationOptions.ContainsKey(m_AllAssets[i].Slot))
                {
                    m_CustomizationOptions[m_AllAssets[i].Slot] = new List<CustomizationAsset>() { null };
                    m_IndexMap[m_AllAssets[i].Slot] = 0;
                }
                m_CustomizationOptions[m_AllAssets[i].Slot].Add(m_AllAssets[i]);
            }

            //setup each ui item
            foreach (var pair in m_CustomizationOptions)
            {
                var item = Instantiate(m_UIItemPrefab, m_UIItemPrefab.transform.parent);
                m_UIItemMap.Add(pair.Key, item);

                item.Label = pair.Key.ToString();
                item.OnClickPrevious = () => EquipItem(pair.Key, m_IndexMap[pair.Key] - 1);
                item.OnClickNext = () => EquipItem(pair.Key, m_IndexMap[pair.Key] + 1);
                EquipItem(pair.Key, 0);
            }

            m_UIItemPrefab.gameObject.SetActive(false);

            //setup color items
            for (int i = 0; i < m_AllTextures.Length; i++)
            {
                var item = Instantiate(m_ColorItemPrefab, m_ColorItemPrefab.transform.parent);
                m_ColorItemMap.Add(item);

                item.Label = m_AllTextures[i].name;
                item.SetIcon(m_AllTextures[i]);

                int aux = i;
                item.OnClick = () => EquipColor(aux);
            }
            m_ColorItemPrefab.gameObject.SetActive(false);
            EquipColor(1);
        }

        private void Update()
        {


            m_Camera.transform.position = Vector3.Lerp(m_CameraTarget_A.position, m_CameraTarget_B.position, m_CameraLerp);
            m_Camera.transform.rotation = Quaternion.Lerp(m_CameraTarget_A.rotation, m_CameraTarget_B.rotation, m_CameraLerp);
            m_Camera.fieldOfView = Mathf.Lerp(30, 10, m_CameraLerp);

            m_CameraLerp = Mathf.Lerp(m_CameraLerp, m_CameraLerpTarget, Time.deltaTime * 8);

            
        }

        private void EquipItem(CustomizationSlot slot, int index)
        {
            if (index < 0)
                index = m_CustomizationOptions[slot].Count - 1;
            if (index >= m_CustomizationOptions[slot].Count)
                index = 0;

            m_IndexMap[slot] = index;

            var uiItem = m_UIItemMap[slot];
            var asset = m_CustomizationOptions[slot][index];

            if (asset == null)
            {
                m_Manager.Unequip(slot);
                uiItem.ItemName = "None";
            }
            else
            {
                m_Manager.EquipAsset(asset);
                uiItem.ItemName = asset.name;
            }
        }

        private void EquipColor(int index)
        {
            m_Manager.SetColorTexture(m_AllTextures[index]);

            for (int i = 0; i < m_ColorItemMap.Count; i++)
                m_ColorItemMap[i].Selected = i == index;
        }
    }
}