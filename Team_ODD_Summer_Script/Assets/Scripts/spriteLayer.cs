using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteLayer : MonoBehaviour
{
    private float x;
    private float y;

    Transform tf;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        x = tf.position.x;
        y = tf.position.y;
        tf.position = new Vector3(x, y, y / 1000.0f);
    }
}
