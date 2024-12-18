using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenuUI : MonoBehaviour
{

    void Awake()
    {
        this.enabled = true;
    }

    private void OnEnable()
    {
        // ?¬?΄ λ‘λ?  ? ?ΈμΆ?
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ?¬ λ‘λ ?΄λ²€νΈ ?΄? 
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.Instance.mg.RanNumGen();
    }

    public void OnClickStartButton()
    {
        GameManager.Instance.is_ingame = true;
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickStoryButton()
    {
        GameManager.Instance.is_contents = true;
        SceneManager.LoadScene("ScrollScene");
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
}
