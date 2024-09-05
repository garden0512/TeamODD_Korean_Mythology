using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TestBigger : MonoBehaviour
{
    public TextMeshProUGUI buttonText;  // TextMeshProUGUI로 수정
    public float scaleMultiplier = 1.2f;  // 확대 비율

    private Vector3 originalScale;

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();  // TextMeshProUGUI로 수정
        }

        originalScale = buttonText.rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.rectTransform.localScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.rectTransform.localScale = originalScale;
    }
}
