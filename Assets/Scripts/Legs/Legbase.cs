using UnityEngine;
using UnityEngine.Events;

public class Legbase : MonoBehaviour
{
    public bool canUse = false;


    public bool leftLegged = false;

    public bool rightLegged = false;

    public float jettisonForce;

    public GameObject jettisonPrefab;

    public UnityEvent onJettison;



    public void Jettison()
    {
        onJettison.Invoke();

        GameObject temp = Instantiate(jettisonPrefab, gameObject.transform.position, gameObject.transform.rotation);

        temp.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * jettisonForce, ForceMode.Impulse);
    }
}
