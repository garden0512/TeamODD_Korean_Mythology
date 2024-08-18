using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Second_Changer : MonoBehaviour
{
    public Image FadeImage;
    public float fadeSpeed = 1f;
    public AudioClip audioground;

    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitAndFadeIn(2f));
    }

    public void ChangeToRealMenu()
    {
        // 페이드 아웃을 실행하고 씬 전환
        StartCoroutine(FadeToScene("SimpleScene"));
    }

    public void EndGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    IEnumerator FadeToScene(string sceneName)
    {
        // 페이드 아웃
        yield return FadeOut();
        // 씬 로드
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOut()
    {
        FadeImage.gameObject.SetActive(true);
        Color imgColor = FadeImage.color;
        while (imgColor.a < 1f)
        {
            imgColor.a += Time.deltaTime * fadeSpeed;
            FadeImage.color = imgColor;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        Color imgColor = FadeImage.color;
        while (imgColor.a > 0f)
        {
            imgColor.a -= Time.deltaTime * fadeSpeed;
            FadeImage.color = imgColor;
            yield return null;
        }
        FadeImage.gameObject.SetActive(false);
    }

    IEnumerator WaitAndFadeIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // 지정된 시간만큼 대기
        StartCoroutine(FadeIn()); // 페이드인 시작
    }
}
