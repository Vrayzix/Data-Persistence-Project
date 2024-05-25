using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField textInput;
    public Text bestScoreText;
    public static string playerName;


    void Start()
    {
        string path1 = Application.persistentDataPath + "/BestScoreData.json";
        string path2 = Application.persistentDataPath + "/PlayerName.json";
        if (File.Exists(path1))
        {
            string json = File.ReadAllText(path1);
            MainManager.BestScore bestScore = JsonUtility.FromJson<MainManager.BestScore>(json);
            bestScoreText.text = bestScore.bestScoreText;
        }
        if (File.Exists(path2))
        {
            string json = File.ReadAllText(path2);
            MainManager.PlayerName playerName = JsonUtility.FromJson<MainManager.PlayerName>(json);
            textInput.text = playerName.PlayerText;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartButton()
    {
        playerName = textInput.text;
        SceneManager.LoadScene("main");
    }
    public void QuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
