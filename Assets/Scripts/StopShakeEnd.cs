using UnityEngine;

using Unity.Cinemachine;

public class StopShakeEnd : MonoBehaviour
{
    public CinemachineCamera cinemachine;
    public float timer = 3.0f;
    public GameObject rocketship;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <=0f)
        {
            CinemachineBasicMultiChannelPerlin noise = cinemachine.GetComponent<CinemachineBasicMultiChannelPerlin>();

            noise.AmplitudeGain = 0f;
            noise.FrequencyGain = 0f;
            Destroy(rocketship);
        }
    }
}
