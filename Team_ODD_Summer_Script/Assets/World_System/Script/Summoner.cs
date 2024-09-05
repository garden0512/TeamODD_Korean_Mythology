using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Summoner : MonoBehaviour
{
    public GameObject shortMobPrefab;
    public GameObject longMobPrefab;
    public SpriteRenderer background;  // 배경 이미지의 SpriteRenderer

    private int shortMobCount = 0;
    private int longMobCount = 0;
    private int maxMobCount = 8;
    private int maxMobPerType = 10;

    private List<GameObject> currentMobs = new List<GameObject>();

    public GameObject Enemy;
    public GameObject[] enemies;
    public GameObject demoObject;

    void Start()
    {
        StartCoroutine(SpawnMobs());
        demoObject.SetActive(false);
    }

    // void Update()
    // {
    //     // 현재 리스트에서 비활성화된 몬스터 제거
    //     for (int i = currentMobs.Count - 1; i >= 0; i--)
    //     {
    //         if (currentMobs[i] == null || !currentMobs[i].activeInHierarchy)
    //         {
    //             // 몬스터가 제거된 경우 리스트에서 삭제
    //             if (currentMobs[i].activeSelf == false)  // Check if it is inactive
    //             {
    //                 currentMobs.RemoveAt(i);
    //                 // 카운트 업데이트
    //                 if (currentMobs[i] == shortMobPrefab)
    //                 {
    //                     shortMobCount--;
    //                 }
    //                 else if (currentMobs[i] == longMobPrefab)
    //                 {
    //                     longMobCount--;
    //                 }
    //             }
    //         }
    //     }
    // }

    IEnumerator SpawnMobs()
    {
        while (shortMobCount < maxMobPerType || longMobCount < maxMobPerType)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length < maxMobCount)
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
        StartCoroutine(CheckAllMobsDead());
    }

    IEnumerator CheckAllMobsDead()
    {
        while (shortMobCount > 0 || longMobCount > 0)
        {
            // 현재 리스트에서 비활성화된 몬스터 제거
            for (int i = currentMobs.Count - 1; i >= 0; i--)
            {
                if (currentMobs[i] == null || !currentMobs[i].activeInHierarchy)
                {
                    currentMobs.RemoveAt(i);

                    // 카운트 업데이트
                    if (currentMobs[i] == shortMobPrefab)
                    {
                        shortMobCount--;
                    }
                    else if (currentMobs[i] == longMobPrefab)
                    {
                        longMobCount--;
                    }
                }
            }

            yield return new WaitForSeconds(1f); // 1초마다 확인
        }

        // 모든 몬스터가 죽은 후 This_is_DEMO 활성화
        if (demoObject != null)
        {
            demoObject.SetActive(true);
        }
        else
        {
            Debug.LogError("This_is_DEMO object is not assigned.");
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
