using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OpenLevel(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
