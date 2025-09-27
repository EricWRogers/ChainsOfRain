using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Health m_enemyHealth;
    private Slider m_healthBar;
    private Transform m_player;

    void Start()
    {
        m_healthBar = GetComponentInChildren<Slider>();
        m_enemyHealth = transform.parent.GetComponent<Health>();
        m_healthBar.maxValue = m_enemyHealth.maxHealth;
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.LookAt(m_player);
        m_healthBar.value = m_enemyHealth.currentHealth;
    }
}
