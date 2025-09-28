using SuperPupSystems.Helper;
using Unity.VisualScripting;
using UnityEngine;

public class LoseMenu : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Character").GetComponent<Health>().outOfHealth.AddListener(LoseScreen);
    }

    public void LoseScreen()
    { 
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Stop all player input
        Time.timeScale = 0;
    }
}
