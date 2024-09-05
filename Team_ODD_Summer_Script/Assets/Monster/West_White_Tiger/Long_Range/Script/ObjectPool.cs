using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public ObjectPool instance;
    public GameObject attackEffectPrefab;
    private Queue<GameObject> effectPool = new Queue<GameObject>();

    public GameObject GetEffect()
    {
        if (effectPool.Count > 0)
        {
            GameObject effect = effectPool.Dequeue();
            effect.SetActive(true);
            return effect;
        }
        else
        {
            return Instantiate(attackEffectPrefab);
        }
    }

    public void ReturnEffect(GameObject effect)
    {
        effect.SetActive(false);
        effectPool.Enqueue(effect);
    }
}
