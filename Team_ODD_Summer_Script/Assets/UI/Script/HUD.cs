using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public enum InfoType { WT, ETC}
    public InfoType type;
    Text myText;
    Slider wtSlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        wtSlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch(type)
        {
            case InfoType.WT:
                float curHp = WT_Control.instance.health;
                float maxHp = WT_Control.instance.maxHealth;
                wtSlider.value = (curHp/maxHp);
                break;
        }
    }
}
