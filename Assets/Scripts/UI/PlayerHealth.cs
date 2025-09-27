using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider m_healthBar;
    private Health m_player;

    void Start()
    {
        m_healthBar = GetComponent<Slider>();
        m_player = GameObject.Find("Character").GetComponent<Health>();
        m_healthBar.maxValue = m_player.maxHealth;
    }

    void Update()
    {
        m_healthBar.value = m_player.currentHealth;
    }
}
