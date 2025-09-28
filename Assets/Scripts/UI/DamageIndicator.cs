using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Transform playerTransform;

    [Header("UI Images (assign arrow images)")]
    public Image upImage;
    public Image rightImage;
    public Image downImage;
    public Image leftImage;

    [Header("Display Settings")]
    public float showDuration = 0.6f;
    public float fadeDuration = 0.25f;
    public float minThreshold = 0.1f;

    private float upTimer, rightTimer, downTimer, leftTimer;

    void Awake()
    {
        SetAllImagesAlpha(0f);
    }

    void Update()
    {
        UpdateIndicator(upImage, ref upTimer);
        UpdateIndicator(rightImage, ref rightTimer);
        UpdateIndicator(downImage, ref downTimer);
        UpdateIndicator(leftImage, ref leftTimer);
    }

    void Start()
    {
        playerTransform = GameObject.Find("Character").transform;
    }

    public void ShowIndicator(Vector3 hitWorldPos)
    {
        if (playerTransform == null) return;

        Vector3 toHit = hitWorldPos - playerTransform.position;
        toHit.y = 0f;
        if (toHit.magnitude < minThreshold) return;

        Vector3 local = playerTransform.InverseTransformDirection(toHit.normalized);

        if (Mathf.Abs(local.z) >= Mathf.Abs(local.x))
        {
            if (local.z > 0) upTimer = showDuration + fadeDuration;
            else downTimer = showDuration + fadeDuration;
        }
        else
        {
            if (local.x > 0) rightTimer = showDuration + fadeDuration;
            else leftTimer = showDuration + fadeDuration;
        }
    }

    void UpdateIndicator(Image img, ref float timer)
    {
        if (img == null) return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer > fadeDuration)
            {
                SetImageAlpha(img, 1f);
            }
            else
            {
                float a = Mathf.Clamp01(timer / fadeDuration);
                SetImageAlpha(img, a);
            }
        }
        else
        {
            SetImageAlpha(img, 0f);
        }
    }

    void SetAllImagesAlpha(float a)
    {
        SetImageAlpha(upImage, a);
        SetImageAlpha(rightImage, a);
        SetImageAlpha(downImage, a);
        SetImageAlpha(leftImage, a);
    }

    void SetImageAlpha(Image img, float a)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = a;
        img.color = c;
        img.enabled = a > 0.001f;
    }
}
