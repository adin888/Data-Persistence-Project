using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    //public string playerName; 
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        //MainManager.Instance.SaveColor();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
    public void NewNameEnter(string playerName)
    {
        playerName = inputField.text;
        if(UIManager.Instance == null)
        {
            UIManager.Instance.playerName = playerName;
            UIManager.Instance.playerNameNow = UIManager.Instance.playerName;
        }
        else
            UIManager.Instance.playerNameNow = playerName;
    }

}
