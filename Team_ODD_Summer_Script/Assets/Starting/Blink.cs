using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blink : MonoBehaviour
{
   float time;
    public TextMeshProUGUI textMeshPro;

    void Update()
    {
        if (time < 0.5f)
        {
            textMeshPro.color = new Color(1, 1, 1, 1 - time);
        }
        else
        {
            textMeshPro.color = new Color(1, 1, 1, time);
            if (time > 1f)
            {
                time = 0;
            }
        }
        time += Time.deltaTime;
    }
}
