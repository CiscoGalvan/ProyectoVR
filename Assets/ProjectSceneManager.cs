using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectSceneManager : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
