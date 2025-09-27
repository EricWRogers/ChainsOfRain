using UnityEngine;

public class LegManager : MonoBehaviour
{
    public static LegManager instance;

    public GameObject rightLeg;
    public GameObject leftLeg;


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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (leftLeg.transform.childCount != 0)
            {


                GameObject gun = leftLeg.transform.GetChild(0).gameObject;
                gun.GetComponent<Gunbase>().Jettison();
                Destroy(gun);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rightLeg.transform.childCount != 0)
            {

                GameObject gun = rightLeg.transform.GetChild(0).gameObject;
                gun.GetComponent<Gunbase>().Jettison();
                Destroy(gun);
            }
        }
    }

    public GameObject OpenLegCheck()
    {
        if (rightLeg.transform.childCount == 0)
        {
            return rightLeg;
        }
        else if (leftLeg.transform.childCount == 0)
        {
            return leftLeg;
        }
        else
        {
            return null;
        }

    }

        public void AttatchLeg(GameObject _leg)
    {
        GameObject leg = OpenLegCheck();

        GameObject new_leg = Instantiate(_leg, leg.transform.parent.parent.GetChild(0).position, leg.transform.parent.parent.GetChild(0).rotation, leg.transform);

        new_leg.GetComponent<Gunbase>().leftHanded = leg.name == "LeftLeg";
        new_leg.GetComponent<Gunbase>().rightHanded = leg.name == "RightLEg";

    }

}
