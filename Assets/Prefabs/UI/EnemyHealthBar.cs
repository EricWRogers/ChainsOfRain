using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Health enemyHealth;
    public Slider healthBar;
    private Transform m_player;

    void Start()
    {
        healthBar.maxValue = enemyHealth.maxHealth;
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.LookAt(m_player);
        healthBar.value = enemyHealth.currentHealth;
    }
}
