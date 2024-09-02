using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start_Manager : MonoBehaviour
{
    public static Start_Manager instance;
    public GameObject Bird;
    public GameObject Bird_2;
    public GameObject Supernova;
    public Button StartB;
    public Button Load;
    public Button Setting;
    public Button Exit;
    public float BirdSpeed = 2.0f;
    public float BirdShrink = 0.5f;
    public float SuperFadeSpeed = 1.0f;
    public Vector3 supernovaRightPosition = new Vector3(8, 0, 0);

    private bool isKeyPressed = false;

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
        // Bird와 Bird_2 오브젝트의 이동 및 축소
        yield return StartCoroutine(MoveAndShrink(Bird));
        yield return StartCoroutine(MoveAndShrink(Bird_2));

        // Supernova 오브젝트의 페이드아웃
        yield return StartCoroutine(FadeOut(Supernova));

        // Supernova 오브젝트 위치 변경 및 좌우 반전
        Supernova.transform.position = supernovaRightPosition;
        Supernova.transform.localScale = new Vector3(-1, 1, 1);  // 좌우반전

        // Supernova 오브젝트의 페이드인
        yield return StartCoroutine(FadeIn(Supernova));

        // 버튼 표시
        ShowButtons();
    }

    IEnumerator MoveAndShrink(GameObject obj)
    {
        Vector3 initialScale = obj.transform.localScale;
        while (obj.transform.localScale.x > 0.01f)
        {
            obj.transform.position += Vector3.one * BirdSpeed * Time.deltaTime;
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

    IEnumerator FadeIn(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += SuperFadeSpeed * Time.deltaTime;
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
}
