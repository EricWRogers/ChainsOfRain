using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string levelName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(levelName);
        }
    }

    public void GoToNext(string _name)
    {
        SceneManager.LoadSceneAsync(_name);
    }
}
