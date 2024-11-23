using UnityEngine;
using UnityEngine.UI;

public class promptUI : MonoBehaviour
{
    public InputField playerInput;
    private string playerPrompt = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerInput = this.gameObject.transform.GetComponentInChildren<InputField>();
        playerPrompt = playerInput.text;
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerPrompt == null ? true : playerPrompt.Length > 0) && Input.GetKeyDown(KeyCode.Return))
        {
            InputPrompt();
        }
    }

    public void InputPrompt()
    {
        playerPrompt = playerInput.text;
        //PlayerPrefs.SetString("CurrentPlayerPrompt", playerPrompt);
        GameManager.Instance.concept = playerPrompt;
        GameManager.Instance.is_mgset = true;
        GameManager.Instance.ui_list[4].gameObject.SetActive(true);
    }
}
