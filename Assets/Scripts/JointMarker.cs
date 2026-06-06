using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JointMarker : MonoBehaviour
{
    [Header("Settings")]
    public string jointName;
    public Color markerColor = Color.green;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.15f;

    private Image markerImage;
    private Vector3 baseScale;

    void Start()
    {
        markerImage = GetComponent<Image>();
        baseScale = transform.localScale;

        if (markerImage != null)
            markerImage.color = markerColor;
    }

    void Update()
    {
        // Pulse animation
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = baseScale * pulse;
    }

    public void SetColor(Color color)
    {
        markerColor = color;
        if (markerImage != null)
            markerImage.color = color;
    }

    public void SetJointName(string name)
    {
        jointName = name;
    }
}