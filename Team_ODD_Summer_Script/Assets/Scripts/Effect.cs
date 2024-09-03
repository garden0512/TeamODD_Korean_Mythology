using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    void OnParticleTrigger()
    {
        
            Short_Control._health -= 250f;
            Long_Control._health -= 250f;
            /*if (other.name.Contains("Short_Mob_Test(Clone)"))
            {
            }

            if (other.name.Contains("Long_Mob_Test(Clone)"))
            {
            }*/
    }

}
