using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : MonoBehaviour
{
    private GameObject m_player;
    private NavMeshAgent m_agent;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRange;
    public int damage;
    public float fireRate;
    private float m_curFireTime;
    public float speed;
    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_player = GameObject.Find("Character");
        m_agent.speed = speed;
    }
    void Start()
    {
        m_agent.SetDestination(m_player.transform.position);
    }
    void Update()
    {
        m_agent.SetDestination(m_player.transform.position);
        float distance = Vector3.Distance(transform.position, m_player.transform.position);

        if (distance <= fireRange)
        {
            transform.LookAt(m_player.transform.position);
            Shoot();

        }
        else
        {
            //m_agent.isStopped = false;
        }
    }

    public void Shoot()
    {
        firePoint.LookAt(m_player.transform);
        m_curFireTime -= Time.deltaTime;
        if (m_curFireTime <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            bullet.GetComponent<Bullet>().damage = damage;
            bullet.GetComponent<Bullet>().hitTarget.AddListener(() => PlayerMovement.instance.transform.parent.GetComponentInChildren<DamageIndicator>().ShowIndicator(transform.position));
            m_curFireTime = fireRate;
        }
            
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
