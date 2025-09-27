using UnityEngine;

public class GunUI : MonoBehaviour
{
    public Gunbase weapon;
    void Start()
    {
        weapon.onJettison.AddListener(DestroyUI);
    }
    void Update()
    {
        
    }
    void DestroyUI()
    {
        Destroy(gameObject);
    }
}
