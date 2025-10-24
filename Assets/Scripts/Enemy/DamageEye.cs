using SuperPupSystems.Helper;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class DamageEye : MonoBehaviour
{
    public Color normalColor;
    public Color damageColor;
    public float emissionIntensity = 1.5f;
    private Renderer m_renderer;
    private Timer m_timer;
    Material m_material;

    void Start()
    {
        m_timer = GetComponent<Timer>();
        m_timer.timeout.AddListener(ResetColor);

        m_renderer = GetComponent<Renderer>();
        m_renderer.material.color = normalColor;
        m_material = m_renderer.material;
        m_material.EnableKeyword("_EMISSION");
        m_material.SetColor("_EmissionColor", normalColor * emissionIntensity);
    }

    public void SetDamageColor()
    {
        m_renderer.material.color = damageColor;
        m_material.SetColor("_EmissionColor", damageColor * emissionIntensity);
        m_timer.StartTimer();
    }

    public void ResetColor()
    {
        m_renderer.material.color = normalColor;
        m_material.SetColor("_EmissionColor", normalColor * emissionIntensity);
    }
}
