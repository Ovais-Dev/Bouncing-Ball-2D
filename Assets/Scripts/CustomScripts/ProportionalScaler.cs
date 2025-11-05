using UnityEngine;
using System.Collections;

/// <summary>
/// Scales a GameObject proportionally over time with various easing styles.
/// </summary>
public class ProportionalScaler : MonoBehaviour
{
    public enum EasingType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        CustomCurve
    }

    [Tooltip("The easing style for the scaling animation.")]
    public EasingType easingType = EasingType.EaseInOut;

    [Tooltip("The target scale to animate to. For example, '2' means double the size.")]
    [Min(0)]
    public float targetScaleFactor = 2f;

    [Tooltip("The duration of the scaling animation in seconds.")]
    public float duration = 1.0f;

    [Tooltip("Define a custom animation curve. X-axis is time (0 to 1), Y-axis is scale progress (0 to 1).")]
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("Should the object scale automatically on start?")]
    public bool scaleOnStart = false;

    private Vector3 initialScale;
    private bool isScaling = false;

    void Awake()
    {
        // Store the original scale of the object.
        initialScale = transform.localScale;
    }

    void Start()
    {
        if (scaleOnStart)
        {
            StartScaling();
        }
    }

    /// <summary>
    /// Starts the scaling animation.
    /// </summary>
    public void StartScaling()
    {
        if (!isScaling)
        {
            StartCoroutine(ScaleRoutine());
        }
    }

    /// <summary>
    /// Resets the object to its original scale instantly.
    /// </summary>
    public void ResetScale()
    {
        StopAllCoroutines();
        isScaling = false;
        transform.localScale = initialScale;
    }

    private IEnumerator ScaleRoutine()
    {
        isScaling = true;
        float elapsedTime = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = initialScale * targetScaleFactor;

        while (elapsedTime < duration)
        {
            // Calculate the progress of the animation from 0 to 1.
            float progress = elapsedTime / duration;

            // Get the eased value based on the selected easing type.
            float easedProgress = GetEasedProgress(progress);

            // Linearly interpolate between the start and end scale using the eased progress.
            transform.localScale = Vector3.Lerp(startScale, endScale, easedProgress);

            // Wait for the next frame.
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set exactly to the target scale.
        transform.localScale = endScale;
        isScaling = false;
    }

    private float GetEasedProgress(float progress)
    {
        switch (easingType)
        {
            case EasingType.Linear:
                return progress;
            case EasingType.EaseIn:
                return progress * progress; // Quadratic ease-in
            case EasingType.EaseOut:
                return 1 - (1 - progress) * (1 - progress); // Quadratic ease-out
            case EasingType.EaseInOut:
                return progress < 0.5f ? 2 * progress * progress : 1 - Mathf.Pow(-2 * progress + 2, 2) / 2;
            case EasingType.CustomCurve:
                // Evaluate the progress on the custom animation curve.
                return scaleCurve.Evaluate(progress);
            default:
                return progress;
        }
    }
}
