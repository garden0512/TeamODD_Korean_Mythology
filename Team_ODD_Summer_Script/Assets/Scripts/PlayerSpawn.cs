using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public GameObject playerspawn;
    public GameObject otherObject;


    void Start()
    {
        playerspawn.SetActive(true);

        otherObject.GetComponent<MovePlayer>().enabled = false;

        Invoke("startsss", 0.5f);
    }

    public void startsss()
    {
        player.SetActive(true);
        playerspawn.SetActive(false);

        otherObject.GetComponent<MovePlayer>().enabled = true;
    }

}
