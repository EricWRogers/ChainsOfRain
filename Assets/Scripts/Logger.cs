using UnityEngine;

public class Logger : MonoBehaviour
{
    public bool logPlayer = false;

    public bool logAi = false;

    public bool logGun = false;

    public static Logger instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Log(string _message, LogType _type) //Just add more. Be sure to add yourself a logtype if it doesnt meet whats availible.
    {
        switch (_type)
        {
            case LogType.Player:
                if (logPlayer) Debug.Log(_message);
                break;
            case LogType.AI:
                if (logAi) Debug.Log(_message);
                break;
            case LogType.Gun:
                if (logGun) Debug.Log(_message);
                break;
        }
    }


    public enum LogType
    {
        Player,
        AI,
        Gun,
    }

}
