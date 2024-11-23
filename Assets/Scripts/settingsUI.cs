using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class settingsUI : MonoBehaviour
{
    void Awake()
    {
        this.enabled = true;
    }

    private void OnEnable()
    {
        // ?”¬?´ ë¡œë“œ?  ?•Œ ?˜¸ì¶?
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ?”¬ ë¡œë“œ ?´ë²¤íŠ¸ ?•´? œ
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //GameManager.Instance.is_ingame = false;
    }
}
