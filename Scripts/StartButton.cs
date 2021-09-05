using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // если нажата клавиша Esc (Escape)
        {
            Application.Quit();    // закрыть приложение
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))  // если нажата клавиша Esc (Escape)
        {
            SceneManager.LoadScene(1);
        }
    }
}
