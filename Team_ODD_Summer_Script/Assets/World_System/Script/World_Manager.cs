using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_Manager : MonoBehaviour
{
    public static World_Manager instance;
    public SpriteRenderer backimage;

    void Awake()
    {
        instance = this;
    }
}
