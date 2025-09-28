using UnityEngine;

public class ExplodingBarrell : MonoBehaviour
{
    public GameObject barrel;
    public GameObject explosion;

    public AudioSource source;

    void Awake()
    {
        barrel.SetActive(true);
        explosion.SetActive(false);
    }

    public void Explode()
    {
        barrel.SetActive(false);
        explosion.SetActive(true);

        source.Play();
    }
}
