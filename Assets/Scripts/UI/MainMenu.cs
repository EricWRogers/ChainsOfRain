using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
