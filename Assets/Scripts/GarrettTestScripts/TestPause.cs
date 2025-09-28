using UnityEngine;

public class TestPause : MonoBehaviour
{
    public PauseMenu pauseMenu;
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenuGo").GetComponentInChildren<PauseMenu>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.TogglePause();
        }
    }
    
}
