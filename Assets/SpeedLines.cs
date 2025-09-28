using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    public bool speedLinesOn;

    void Update()
    {
        if (speedLinesOn)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
