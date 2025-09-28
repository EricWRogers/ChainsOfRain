using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.Events;

public abstract class Gunbase : MonoBehaviour
{
    public bool canShoot = false;
    public bool leftHanded = false;

    public bool rightHanded = false;

    public bool infiniteAmmo;
    public bool reloadable = true;
    public bool press = false;
    public bool hold = false;

    public GameObject bulletPrefab;

    public GameObject jettisonPrefab;


    public float jettisonForce;
    public int maxAmmo = 100;
    public int magazineAmmo = 10;
    [HideInInspector]
    public int ammo;

    public int damage = 1;

    public Transform firingPoint;

    public UnityEvent onFire;

    public UnityEvent onReload;


    [Header("UI")]
    public GameObject uIPrefab;
    public RectTransform rightUI;
    public RectTransform leftUI;
    private bool m_spawnedUI;

    private KeyCode activeHand => leftHanded ? KeyCode.Mouse0 : KeyCode.Mouse1;

    private InputType inputMode => press ? InputType.GetKeyDown : InputType.GetKey;

    public UnityEvent onJettison;

    public void Start()
    {
        ammo = magazineAmmo;
        
    }


    public void Update()
    {
        if (canShoot)
        {
            if (rightHanded && !m_spawnedUI)
            {
                rightUI = WeaponManager.instance.rightArm.transform.parent.GetComponentInChildren<RectTransform>();
                GameObject uI = Instantiate(uIPrefab, rightUI);
                uI.GetComponent<GunUI>().weapon = this;
                m_spawnedUI = true;
            }
            if (leftHanded && !m_spawnedUI)
            {
                leftUI = WeaponManager.instance.leftArm.transform.parent.GetComponentInChildren<RectTransform>();
                GameObject uI = Instantiate(uIPrefab, leftUI);
                uI.GetComponent<GunUI>().weapon = this;
                m_spawnedUI = true;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                ReleaseFiring();
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                ReleaseFiring();
            }
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
            {
                firingPoint.LookAt(hit.point);
            }
            switch (inputMode)
            {
                case InputType.GetKeyDown:
                    if (Input.GetKeyDown(activeHand))
                    {
                        Logger.instance.Log("Firing!", Logger.LogType.Gun);
                        Fire(firingPoint, bulletPrefab);
                    }
                    break;
                case InputType.GetKey:
                    if (Input.GetKey(activeHand))
                    {
                        Logger.instance.Log("Firing! (holding)", Logger.LogType.Gun);
                        Fire(firingPoint, bulletPrefab);
                    }
                    break;
            }

            if (reloadable && Input.GetKeyDown(KeyCode.R) && (infiniteAmmo || maxAmmo > 0)) //For now ill be lazy. Please set your max ammo to be divisible by your magazine ammo.
            {
                onReload.Invoke();
                ammo = magazineAmmo;
            }
        }
    }

    public abstract void Fire(Transform _firingPoint, GameObject _bulletPrefab);

    public abstract void ReleaseFiring();


    public void UpdateDamage(int _value, GameObject _bullet)
    {
        _bullet.GetComponent<Bullet>().damage = _value;
    }


    public void Jettison()
    {
        onJettison.Invoke();

        GameObject temp = Instantiate(jettisonPrefab, gameObject.transform.position, gameObject.transform.rotation);

        temp.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * jettisonForce, ForceMode.Impulse);
    }
    private enum InputType
    {
        GetKey,
        GetKeyDown
    }

}
