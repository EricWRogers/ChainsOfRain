using UnityEngine;

public class TestPause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool m_paused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        if (!m_paused)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Stop all player input
            Time.timeScale = 0;
            m_paused = true;
        }
        else
        {
            m_paused = false;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Stop all player input
            Time.timeScale = 1;
        }
    }
}
