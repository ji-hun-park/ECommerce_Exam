using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class minigameUI : MonoBehaviour
{
    public Image[] img_list;
    public Text[] txt_list;
    public Button[] btn_list;

    void Awake()
    {
        this.enabled = true;
        img_list = GetComponentsInChildren<Image>();
        txt_list = GetComponentsInChildren<Text>();
        btn_list = GetComponentsInChildren<Button>();
        UpdateImages();
        ButtonSet();
    }

    public void UpdateImages()
    {
        img_list[3].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[0]];
        img_list[4].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[1]];
        img_list[5].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[2]];
        img_list[6].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[3]];
        img_list[7].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[4]];
        img_list[8].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[5]];
        img_list[9].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[6]];
        img_list[10].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum8[7]];
    }

    public void ButtonSet()
    {
        btn_list[1].onClick.AddListener(() => OnClickImageButton(1));
        btn_list[2].onClick.AddListener(() => OnClickImageButton(2));
        btn_list[3].onClick.AddListener(() => OnClickImageButton(3));
        btn_list[4].onClick.AddListener(() => OnClickImageButton(4));
        btn_list[5].onClick.AddListener(() => OnClickImageButton(5));
        btn_list[6].onClick.AddListener(() => OnClickImageButton(6));
        btn_list[7].onClick.AddListener(() => OnClickImageButton(7));
        btn_list[8].onClick.AddListener(() => OnClickImageButton(8));
    }

    private void OnEnable()
    {
        // ?¬?΄ λ‘λ?  ? ?ΈμΆ?
        //SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateImages();
        ButtonSet();
    }

    private void OnDisable()
    {
        // ?¬ λ‘λ ?΄λ²€νΈ ?΄? 
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.gameObject.SetActive(true);
        this.enabled = true;
        UpdateImages();
        ButtonSet();
    }

    public void OnClickImageButton(int num)
    {
        GameManager.Instance.IMGNUM = num;
        GameManager.Instance.ui_list[3].gameObject.SetActive(true);
    }
}
