using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Start_Manager : MonoBehaviour
{
    public static Start_Manager instance;
    public GameObject Bird;
    public GameObject Bird_2;
    public GameObject Supernova;
    public GameObject ClickAny;
    public Button StartB;
    public Button Load;
    public Button Setting;
    public Button Exit;
    public float BirdSpeed = 2.0f;
    public float BirdShrink = 0.5f;
    public float SuperFadeSpeed = 1.0f;
    private bool isKeyPressed = false;
    [Header("Recognition Info")]
    public float trx = 0f;


    void Start()
    {
        // 초기 상태 설정
        StartB.gameObject.SetActive(false);
        Load.gameObject.SetActive(false);
        Setting.gameObject.SetActive(false);
        Exit.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isKeyPressed && Input.anyKeyDown)
        {
            isKeyPressed = true;
            StartCoroutine(StartSequence());
        }
    }

    IEnumerator StartSequence()
    {
        Supernova.transform.DOMoveX(trx,1f);
        Supernova.transform.DORotate(new Vector3(0, 180, 0), 1);
        Bird.transform.DOMove(new Vector3(1, 1, 0), 1);
        ClickAny.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() => ClickAny.SetActive(false));
        yield return null;
        ShowButtons();
    }

    IEnumerator MoveAndShrink(GameObject obj, Vector2 direction)
    {
        Vector3 initialScale = obj.transform.localScale;
        while (obj.transform.localScale.x > 0.01f)
        {
            obj.transform.position += (Vector3)direction * BirdSpeed * Time.deltaTime;
            obj.transform.localScale -= Vector3.one * BirdShrink * Time.deltaTime;
            yield return null;
        }
        obj.SetActive(false);
    }

    IEnumerator FadeOut(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= SuperFadeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    void ShowButtons()
    {
        StartB.gameObject.SetActive(true);
        Load.gameObject.SetActive(true);
        Setting.gameObject.SetActive(true);
        Exit.gameObject.SetActive(true);

        // 버튼 애니메이션: 각 버튼이 확대되면서 등장
        StartCoroutine(AnimateButton(StartB));
        StartCoroutine(AnimateButton(Load));
        StartCoroutine(AnimateButton(Setting));
        StartCoroutine(AnimateButton(Exit));
    }

    IEnumerator AnimateButton(Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        rectTransform.localScale = initialScale;

        while (rectTransform.localScale.x < targetScale.x)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * 5f);
            yield return null;
        }
    }

    public void StartPlay()
    {
        StartCoroutine(StartScene("SampleScene"));
    }

    public void EndGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    IEnumerator StartScene(string sceneName)
    {
        yield return null;
        SceneManager.LoadScene(sceneName);
    }
}
