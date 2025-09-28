using UnityEngine;

public class GunUI : MonoBehaviour
{
    public Gunbase weapon;
   public void Start()
    {
        weapon.onJettison.AddListener(DestroyUI);
    }
    void Update()
    {
        
    }
    void DestroyUI()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
