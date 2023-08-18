using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomizationColorItem : MonoBehaviour
{
    [SerializeField] private Text m_Label;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Image m_Selected;
    [SerializeField] private Button m_Button;

    public System.Action OnClick { get; set; }

    public bool Selected
    {
        get => m_Selected.enabled;
        set => m_Selected.enabled = value;
    }

    public string Label
    {
        get => m_Label.text;
        set => m_Label.text = value;
    }

    private void Awake()
    {
        Selected = false;
        m_Button.onClick.AddListener(_OnClick);
    }

    private void _OnClick()
    {
        OnClick?.Invoke();
    }

    public void SetIcon(Texture2D texture)
    {
        Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
        SetIcon(spr);
    }

    public void SetIcon(Sprite spr)
    {
        m_Icon.sprite = spr;
    }

    private void OnDestroy()
    {
        if (m_Icon.sprite != null)
            Destroy(m_Icon.sprite);
    }
}
