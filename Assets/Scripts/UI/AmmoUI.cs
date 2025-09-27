using TMPro;
using UnityEngine;

public class AmmoUI : GunUI
{
    public TextMeshProUGUI ammoCounterText;

    void Start()
    {
        weapon.onJettison.AddListener(DestroyUI);
    }
    void Update()
    {
        ammoCounterText.text = weapon.ammo + "\n―\n" + weapon.maxAmmo;
        if (weapon.leftHanded)
        {
            ammoCounterText.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
    void DestroyUI()
    {
        Destroy(gameObject);
    }

}
