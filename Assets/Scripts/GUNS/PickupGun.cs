using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickupGun : MonoBehaviour
{
    public int health = 10;
    public GunType gun;
    public float spinSpeed = 90f;
    public float hoverAmplitude = 0.25f;
    public float hoverFrequency = 2f;
    public LayerMask groundMask;
    public float hoverHeight = 1.36f;

    public float spinSpeed;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float heightOffset = 1.0f;
    public LayerMask mask;

    public float timeForce = 1.0f;
    public float dissolveScale = 0.25f;
    private float rayLength = 0.75f;
    private float fadeOutDelay = 1f;
    private Timer timer;
    private bool letsMoveIt = false;
    private int repeat = 0;
    [SerializeField]
    private Renderer gunMat;
    [SerializeField]
    private Renderer armMat;



    Vector3 groundPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool didwork = WeaponManager.instance.AttatchGun(gun);
            if (didwork)
            {
                AddHealth();
                Destroy(gameObject);
            }
        }
        else
        {
            basePos = gameObject.transform.root.transform.position;
        }
    }

    private void Update()
    {
        // Spin
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

    private void Start()
    {
        timer = gameObject.GetComponent<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, (int)mask))
        {
            letsMoveIt = true;
        }
        else
        {
            transform.Translate(Vector3.down * timeForce * Time.deltaTime);
        }

        if (letsMoveIt)
        {
            HoverAndSpin();
            repeat += 1;
            //start timer
            if (timer != null && repeat == 1)
                timer.StartTimer();
        }
    }

    public void AddHealth()
    {
        if (groundPos == new Vector3(0f, 0f, 0f))
        {
            groundPos = transform.position;
        }
        float newY = groundPos.y + heightOffset + Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
        transform.position = new Vector3(groundPos.x, newY, groundPos.z); // only need to hover on the y axis
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    public void Disappear()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutDelay);

        float time = 0;
        float cv = armMat.material.GetFloat("_Clipping_Value");
        while (time < 2.85f)
        {
            armMat.material.SetFloat("_Clipping_Value", cv * dissolveScale + Time.deltaTime);
            gunMat.material.SetFloat("_Clipping_Value", cv * dissolveScale + Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
