using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;

public class Blink : MonoBehaviour
{
   float time;
    public TextMeshProUGUI textMeshPro;
    private bool isKeyPressed = false;
    public GameObject ClickAny;

    void Update()
    {
        if (isKeyPressed)
        {
            return;
        }
        if (time < 0.5f)
        {
            textMeshPro.color = new Color(0, 0, 0, 1 - time);
        }
        else
        {
            textMeshPro.color = new Color(0, 0, 0, time);
            if (time > 1f)
            {
                time = 0;
            }
        }
        time += Time.deltaTime;

        if (!isKeyPressed && Input.anyKeyDown)
        {
            isKeyPressed = true;
        }
    }
}
