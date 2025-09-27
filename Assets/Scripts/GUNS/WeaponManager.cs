using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;


    public GameObject rightArm;
    public GameObject leftArm;

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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(leftArm.transform.childCount != 0)
            {
                

                GameObject gun = leftArm.transform.GetChild(0).gameObject;
                gun.GetComponent<Gunbase>().onJettison.Invoke();
                Destroy(gun);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rightArm.transform.childCount != 0)
            {

                GameObject gun = rightArm.transform.GetChild(0).gameObject;
                gun.GetComponent<Gunbase>().onJettison.Invoke();
                Destroy(gun);
            }
        }
    }

    public GameObject OpenArmCheck()
    {
        if (rightArm.transform.childCount == 0)
        {
            return rightArm;
        }
        else if (leftArm.transform.childCount == 0)
        {
            return leftArm;
        }
        else
        {
            return null;
        }
    }

    public void AttatchGun(GameObject _gun)
    {
        GameObject arm = OpenArmCheck();

       GameObject gun = Instantiate(_gun, arm.transform.parent.parent.GetChild(0).position, arm.transform.parent.parent.GetChild(0).rotation, arm.transform);

        gun.GetComponent<Gunbase>().leftHanded = arm.name == "LeftArm";
        gun.GetComponent<Gunbase>().rightHanded = arm.name == "RightArm";

    }
}
