using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool m_paused;
    public void Resume()
    {
        m_paused = false;
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
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
    
    public void TogglePause()
    {
        if (!m_paused)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Stop all player input
            Time.timeScale = 0;
            m_paused = true;
        }
        else
        {
            m_paused = false;
            transform.GetChild(0).gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Stop all player input
            Time.timeScale = 1;
        }
    }
}
