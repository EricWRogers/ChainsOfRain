
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float sensitivity;
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
