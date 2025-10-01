using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public GameObject unbrokenBox;
    public GameObject brokenBox;
    
    void Awake()
    {
        unbrokenBox.SetActive(true);
        brokenBox.SetActive(false);
    }

    public void BreakBox()
    {
        unbrokenBox.SetActive(false);
        brokenBox.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 4.0f);
    }
}
