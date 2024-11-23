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
        //GameManager.Instance.is_ingame = false;
    }
}
