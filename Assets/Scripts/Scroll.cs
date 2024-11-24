using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Scroll : MonoBehaviour
{
    public GameObject scrollView; // ScrollView ����
    public Transform content; // Viewport�� Content
    public Sprite imageSprite; // ����� �̹��� Sprite
    public Vector2 imageSize = new Vector2(1300, 1200); // �̹��� ũ��
    public string settextContent; // �ؽ�Ʈ ����
    public int fontSize = 30; // �ؽ�Ʈ ũ��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        scrollView = GameObject.Find("ScrollView");
        // 1. ScrollView�� Content ã��
        content = scrollView.transform.Find("Viewport/Content");

        if (content == null)
        {
            Debug.LogError("Content�� ã�� �� �����ϴ�. ScrollView ������ Ȯ���ϼ���.");
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
        // 2. ���ο� �̹��� GameObject ����
        GameObject newImage = new GameObject("DynamicImage", typeof(Image));
        Image imageComponent = newImage.GetComponent<Image>();
        imageComponent.sprite = imageSprite; // �̹��� ����
        imageComponent.raycastTarget = false; // �ʿ信 ���� Ŭ�� �Ұ� ����

        // 3. RectTransform ����
        RectTransform imageRect = newImage.GetComponent<RectTransform>();
        imageRect.SetParent(content, false); // Content�� �ڽ����� ����
        imageRect.sizeDelta = imageSize; // �̹��� ũ�� ����

        // 4. ��ġ ���� (��: Content�� �ϴܿ� �߰�)
        int childCount = content.childCount; // ���� �ڽ� ��
        float spacing = 10f; // �̹��� �� ����
        imageRect.anchoredPosition = new Vector2(0, -childCount * (imageSize.y + spacing));

        // 5. �̹����� �ڽ����� Text �߰�
        GameObject newText = new GameObject("DynamicText", typeof(Text));
        Text textComponent = newText.GetComponent<Text>();

        // �ؽ�Ʈ ���� ����
        textComponent.text = textContent;
        //textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); // �⺻ Arial ��Ʈ ���
        textComponent.font = Resources.Load<Font>("neodgm");
        textComponent.fontSize = fontSize;
        textComponent.color = Color.black; // �ؽ�Ʈ ����

        // RectTransform ����
        RectTransform textRect = newText.GetComponent<RectTransform>();
        textRect.SetParent(newImage.transform, false); // �̹����� �ڽ����� ����
        textRect.anchorMin = new Vector2(0, 0); // �ؽ�Ʈ�� �̹��� �߾ӿ� ��ġ
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
