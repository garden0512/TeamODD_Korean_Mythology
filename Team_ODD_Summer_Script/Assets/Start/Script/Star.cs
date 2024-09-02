using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    public GameObject GameObject;
    public float trx = 0f;
    void Start()
    {
        GameObject.transform.DOMoveX(trx,1f).SetEase(Ease.InElastic);
        GameObject.transform.DORotate(new Vector3(0, 180, 0), 1);
    }

    void Update()
    {
        
    }
}
