using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        #if UNITY_EDITOR
        // This code only runs in the Unity Editor
        EditorApplication.isPlaying = false;
        #else
        // This code runs in a standalone build
        Application.Quit();
        #endif  
    }
}
