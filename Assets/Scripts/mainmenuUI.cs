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
        // ?��?�� 로드?�� ?�� ?���?
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ?�� 로드 ?��벤트 ?��?��
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

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
}
