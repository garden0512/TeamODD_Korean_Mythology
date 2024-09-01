using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.AI;

public class Summoner : MonoBehaviour
{
    public GameObject shortMobPrefab;
    public GameObject longMobPrefab;
    public Transform spawnArea;

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
        // 맵 안에서 랜덤 위치 선택
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
            Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2),
            0f
        );

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

        // // 만약 오브젝트가 6개를 넘었다면 가장 먼저 소환된 오브젝트 제거
        // if (currentMobs.Count > maxMobCount)
        // {
        //     Destroy(currentMobs[0]);
        //     currentMobs.RemoveAt(0);
        // }
    }
}
