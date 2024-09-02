using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
