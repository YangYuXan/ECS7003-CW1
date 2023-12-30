using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public bool isEasy;

    // Start is called before the first frame update
    public void GameStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameQuit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void EasyLevel()
    {
        isEasy = true;
        Debug.Log(isEasy);
    }

    public void DifficultLevel()
    {
        isEasy = false;
        Debug.Log(isEasy);
    }

}
