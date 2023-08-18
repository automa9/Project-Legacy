using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomizationItem : MonoBehaviour
{
    [SerializeField] private Text m_Label;
    [SerializeField] private Text m_ItemName;

    [SerializeField] private Button m_PreviousButton;
    [SerializeField] private Button m_NextButton;

    public System.Action OnClickPrevious { get; set; }
    public System.Action OnClickNext { get; set; }

    public string Label
    {
        get => m_Label.text;
        set => m_Label.text = value;
    }

    public string ItemName
    {
        get => m_ItemName.text;
        set => m_ItemName.text = value;
    }

    private void Awake()
    {
        m_PreviousButton.onClick.AddListener(_OnClickPrevious);
        m_NextButton.onClick.AddListener(_OnClickNext);
    }

    private void _OnClickPrevious()
    {
        OnClickPrevious?.Invoke();
    }

    private void _OnClickNext()
    {
        OnClickNext?.Invoke();
    }
}
