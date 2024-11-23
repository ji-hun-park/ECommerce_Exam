using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class submenuUI : MonoBehaviour
{
    void Awake()
    {
        this.enabled = true;
    }

    private void OnEnable()
    {
        // ?î¨?ù¥ Î°úÎìú?ê† ?ïå ?ò∏Ï∂?
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ?î¨ Î°úÎìú ?ù¥Î≤§Ìä∏ ?ï¥?†ú
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.gameObject.SetActive(true);
    }

    public void OnClickReturnButton()
    {
        GameManager.Instance.is_ingame = false;
        GameManager.Instance.is_rannum = true;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnClickSaveButton()
    {
        Debug.Log("¿˙¿Âµ«æ˙Ω¿¥œ¥Ÿ!");
    }

    public void OnClickCloseButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
