using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteLayer : MonoBehaviour
{/*
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
    }*/

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // y 좌표에 따라 Order in Layer를 설정
        if(transform.position.y >= 0)
        {

            spriteRenderer.sortingOrder = Mathf.RoundToInt((transform.position.y + 1) * 100);
        }
        else
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);

        }
    }
}
