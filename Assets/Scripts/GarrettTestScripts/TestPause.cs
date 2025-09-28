using UnityEngine;

public class TestPause : MonoBehaviour
{
    public PauseMenu pauseMenu;
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenuPrefab").GetComponentInChildren<PauseMenu>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.TogglePause();
        }
    }
    
}
