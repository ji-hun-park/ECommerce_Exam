using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Scroll : MonoBehaviour
{
    public GameObject scrollView; // ScrollView 참조
    public Transform content; // Viewport의 Content
    public Sprite imageSprite; // 사용할 이미지 Sprite
    public Vector2 imageSize = new Vector2(1300, 1200); // 이미지 크기
    public string settextContent; // 텍스트 내용
    public int fontSize = 30; // 텍스트 크기

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        scrollView = GameObject.Find("ScrollView");
        // 1. ScrollView의 Content 찾기
        content = scrollView.transform.Find("Viewport/Content");

        if (content == null)
        {
            Debug.LogError("Content를 찾을 수 없습니다. ScrollView 구조를 확인하세요.");
            return;
        }
    }

    // Update is called once per frame
    void OnEnable()
    {
        for (int i = 0; i < GameManager.Instance.stories.Count; i++)
        {
            settextContent = GameManager.Instance.stories[i];
            CreateImageWithText(settextContent);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < GameManager.Instance.stories.Count; i++)
        {
            settextContent = GameManager.Instance.stories[i];
            CreateImageWithText(settextContent);
        }
    }


    public void CreateImageWithText(string textContent)
    {
        // 2. 새로운 이미지 GameObject 생성
        GameObject newImage = new GameObject("DynamicImage", typeof(Image));
        Image imageComponent = newImage.GetComponent<Image>();
        imageComponent.sprite = imageSprite; // 이미지 설정
        imageComponent.raycastTarget = false; // 필요에 따라 클릭 불가 설정

        // 3. RectTransform 설정
        RectTransform imageRect = newImage.GetComponent<RectTransform>();
        imageRect.SetParent(content, false); // Content의 자식으로 설정
        imageRect.sizeDelta = imageSize; // 이미지 크기 설정

        // 4. 위치 조정 (예: Content의 하단에 추가)
        int childCount = content.childCount; // 기존 자식 수
        float spacing = 10f; // 이미지 간 간격
        imageRect.anchoredPosition = new Vector2(0, -childCount * (imageSize.y + spacing));

        // 5. 이미지의 자식으로 Text 추가
        GameObject newText = new GameObject("DynamicText", typeof(Text));
        Text textComponent = newText.GetComponent<Text>();

        // 텍스트 내용 설정
        textComponent.text = textContent;
        //textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); // 기본 Arial 폰트 사용
        textComponent.font = Resources.Load<Font>("neodgm");
        textComponent.fontSize = fontSize;
        textComponent.color = Color.black; // 텍스트 색상

        // RectTransform 설정
        RectTransform textRect = newText.GetComponent<RectTransform>();
        textRect.SetParent(newImage.transform, false); // 이미지의 자식으로 설정
        textRect.anchorMin = new Vector2(0, 0); // 텍스트를 이미지 중앙에 배치
        textRect.anchorMax = new Vector2(1, 1);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
    }

    public void OnClickReturnButton()
    {
        GameManager.Instance.is_ingame = false;
        GameManager.Instance.is_rannum = true;
        GameManager.Instance.is_contents = false;
        SceneManager.LoadScene("MainMenuScene");
    }
}
