using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float volume;
    public float sensitivity;

    public float Volume { get; set; }
    public float Sensitivity { get; set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
