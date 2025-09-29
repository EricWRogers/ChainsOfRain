
using SuperPupSystems.Helper;

using UnityEngine;

public class JettesonExplosion : MonoBehaviour
{

    public GameObject explosion;
    public float raidusExplosion = 5f;
    public bool blownUp = false;
    public float time;
    public float timeToDestory = 1.5f;

    public int damage = 10;

    private GameObject exploOBJ;
    public int collideCount;
    public Collider trigger;

    void Start()
    {
        trigger = gameObject.GetComponent<Collider>();
    }


    void Update()
    {
        if (time > 0.1f)
        {
            trigger.enabled = true;
        }
        else
        {
            trigger.enabled = false;
        }
        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            return;
            
        time = 0f;
        if (collideCount > 0) return;
        collideCount += 1;
        blownUp = true;
        exploOBJ = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(exploOBJ, 1.0f);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, raidusExplosion, Vector3.up);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Health>().Damage(damage);
            }

        }
        Destroy(gameObject);
        
    }
}
