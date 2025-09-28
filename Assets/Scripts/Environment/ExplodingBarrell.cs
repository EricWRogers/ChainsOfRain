using UnityEngine;

public class ExplodingBarrell : MonoBehaviour
{
    public GameObject barrel;
    public GameObject explosion;
    public GameObject vfxExplosion;

    public AudioSource source;

    void Awake()
    {
        barrel.SetActive(true);
        explosion.SetActive(false);
        vfxExplosion.SetActive(false);
    }

    public void Explode()
    {
        barrel.SetActive(false);
        explosion.SetActive(true);
        vfxExplosion.SetActive(true);

        source.Play();
    }
}
