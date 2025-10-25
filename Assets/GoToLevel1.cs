using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToLevel1 : MonoBehaviour
{
    public void GoToLevelOne()
    {
        SceneManager.LoadScene("LevelOne");
    }
}
