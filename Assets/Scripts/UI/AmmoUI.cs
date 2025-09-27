using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public Gunbase Weapon;
    public TextMeshProUGUI ammoCounterText;

    void Start()
    {
        Weapon.onJettison.AddListener(DestroyUI);
    }
    void Update()
    {
        ammoCounterText.text = Weapon.ammo + "\n―\n" + Weapon.maxAmmo;
        if (Weapon.leftHanded)
        {
            ammoCounterText.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
    void DestroyUI()
    {
        Destroy(gameObject);
    }

}
