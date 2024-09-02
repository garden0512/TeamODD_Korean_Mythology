using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.AI;

public class Summoner : MonoBehaviour
{
    public GameObject shortMobPrefab;
    public GameObject longMobPrefab;
    public SpriteRenderer background;  // 배경 이미지의 SpriteRenderer

    private int shortMobCount = 0;
    private int longMobCount = 0;
    private int maxMobCount = 6;
    private int maxMobPerType = 20;

    private List<GameObject> currentMobs = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnMobs());
    }

    IEnumerator SpawnMobs()
    {
        while (shortMobCount < maxMobPerType || longMobCount < maxMobPerType)
        {
            if (currentMobs.Count < maxMobCount)
            {
                SpawnMob();
            }

            yield return new WaitForSeconds(3f);
        }
    }

    void SpawnMob()
    {
        if (shortMobCount < maxMobPerType && longMobCount < maxMobPerType)
        {
            // Short_Mob_Test와 Long_Mob_Test를 랜덤하게 선택
            GameObject mobPrefab = Random.Range(0, 2) == 0 ? shortMobPrefab : longMobPrefab;

            // 선택된 mobPrefab을 소환
            SpawnSelectedMob(mobPrefab);
        }
        else if (shortMobCount < maxMobPerType)
        {
            SpawnSelectedMob(shortMobPrefab);
        }
        else if (longMobCount < maxMobPerType)
        {
            SpawnSelectedMob(longMobPrefab);
        }
    }

    void SpawnSelectedMob(GameObject mobPrefab)
    {
        // 경계 내 무작위 위치 선택
        Vector3 spawnPosition = GetRandomPositionWithinBounds();

        GameObject newMob = Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
        currentMobs.Add(newMob);

        if (mobPrefab == shortMobPrefab)
        {
            shortMobCount++;
        }
        else if (mobPrefab == longMobPrefab)
        {
            longMobCount++;
        }
    }

    Vector3 GetRandomPositionWithinBounds()
    {
        // 배경 이미지의 경계를 기준으로 랜덤 위치를 선택
        Bounds bounds = background.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0f
        );
    }
}
