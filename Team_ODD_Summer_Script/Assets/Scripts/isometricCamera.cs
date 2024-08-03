using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isometricCamera : MonoBehaviour
{
    [SerializeField]

    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetZ = 0f;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + offsetX,
            player.transform.position.y + offsetY, player.transform.position.z + offsetZ);
    }
}
