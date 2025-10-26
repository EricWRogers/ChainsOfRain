using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Health m_enemyHealth;
    public Slider m_healthBar;
    public Transform m_player;

    void Start()
    {
        // m_healthBar = gameObject.transform.root.GetComponentInChildren<Slider>();
        // m_enemyHealth = gameObject.transform.root.GetComponent<Health>();
        m_healthBar.maxValue = m_enemyHealth.maxHealth;
        m_player = PlayerMovement.instance.gameObject.transform;
    }

    void Update()
    {
        transform.LookAt(m_player);
        m_healthBar.value = m_enemyHealth.currentHealth;
    }
}
