using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Summoner : MonoBehaviour
{
    public GameObject Tempo_EnemyPrefab;
    public GameObject WT_Test;
    public SpriteRenderer Background;
    public Transform spawnPoint;
    public float spawnInterval = 5.0f;
    private int Kill_Mob = 0;
    private isometricCamera Bosscam;
//개발자 확인용
    private bool waitingForY = false;
    private bool waitingForG = false;
    private void Start()
    {
        Bosscam = Camera.main.GetComponent<isometricCamera>();
        StartSpawning();
    }

    public void StartSpawning()
    {
        InvokeRepeating("SpawnEnemy", 2.0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject enemyPrefab = Tempo_EnemyPrefab;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Bounds bounds = Background.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x,y);
    }

    public void Mob_kill()
    {
        Kill_Mob ++;
        if(Kill_Mob >= 10)
        {
            Boss_Active();
        }
    }

    void Boss_Active()
    {
        WT_Test.transform.position = spawnPoint.position;
        WT_Test.SetActive(true);
        Bosscam.FocusOnPosition(new Vector3(spawnPoint.position.x, spawnPoint.position.y, Bosscam.transform.position.z));
        StartCoroutine(Camera_Delay());
    }

    IEnumerator Camera_Delay()
    {
        yield return new WaitForSeconds(2.0f);
        Bosscam.ResumeFollowingPlayer();
    }

    public void StopSpawn()
    {
        CancelInvoke("SpawnEnemy");
    }

//개발자용 확인 단축키
    private void Update()
    {
        // y 키를 눌렀는지 확인
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Y");
            if (!waitingForY)
            {
                waitingForY = true;
                waitingForG = false; // G 키 입력 대기 상태 초기화
            }
        }

        // g 키를 눌렀는지 확인
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("G");
            if (waitingForY)
            {
                Kill_Mob++;
                Debug.Log(Kill_Mob);
                waitingForY = false; // y 키 입력 대기 상태 초기화
                waitingForG = false; // G 키 입력 대기 상태 초기화
            }
        }
    }
}
