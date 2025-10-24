using SuperPupSystems.Helper;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class DamageEye : MonoBehaviour
{
    public Color normalColor;
    public Color damageColor;
    private Renderer m_renderer;
    private Timer m_timer;

    void Start()
    {
        m_timer = GetComponent<Timer>();
        m_timer.timeout.AddListener(ResetColor);
        
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.color = normalColor;
    }

    public void SetDamageColor()
    {
        m_renderer.material.color = damageColor;
        m_timer.StartTimer();
    }

    public void ResetColor()
    {
        m_renderer.material.color = normalColor;
    }
}
