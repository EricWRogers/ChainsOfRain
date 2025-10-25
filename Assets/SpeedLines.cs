using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    public bool speedLinesOn;
    
    public GameObject lines;
    void Update()
    {
        if (speedLinesOn)
        {
            lines.gameObject.SetActive(true);
            //transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            lines.gameObject.SetActive(false);
        }
    }

}
