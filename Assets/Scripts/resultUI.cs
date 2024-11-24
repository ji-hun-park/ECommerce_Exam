using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class resultUI : MonoBehaviour
{
    public Text board;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        board = GameObject.Find("PromptResponse").GetComponent<Text>();
    }

    void Update()
    {
        if(GameManager.Instance.is_catch) board.text = GameManager.Instance.APIResponse;
    }

    public void OnClickTitleButton()
    {
        GameManager.Instance.stories.Add(GameManager.Instance.APIResponse);
        GameManager.Instance.APIResponse = null;
        GameManager.Instance.is_ingame = false;
        GameManager.Instance.is_rannum = true;
        GameManager.Instance.is_catch = false;
        SceneManager.LoadScene("MainMenuScene");
    }
}
