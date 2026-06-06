using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PoseManager : MonoBehaviour
{
    [Header("References")]
    public GameObject jointMarkerPrefab;
    public RectTransform canvasRect;

    private string[] trackedJoints = new string[]
    {
        "nose",
        "left_shoulder",  "right_shoulder",
        "left_elbow",     "right_elbow",
        "left_wrist",     "right_wrist"
    };

    private Dictionary<string, RectTransform> markers = new Dictionary<string, RectTransform>();

    void Start()
    {
        foreach (string joint in trackedJoints)
        {
            GameObject marker = Instantiate(jointMarkerPrefab, canvasRect);
            marker.name = joint;
            marker.SetActive(false);
            markers[joint] = marker.GetComponent<RectTransform>();
        }
    }

    void Update()
    {
        if (WebSocketClient.Instance == null || !WebSocketClient.Instance.IsConnected)
            return;

        var joints = WebSocketClient.Instance.JointPositions;

        foreach (string joint in trackedJoints)
        {
            if (!markers.ContainsKey(joint)) continue;

            if (joints.ContainsKey(joint))
            {
                Vector2 normalized = joints[joint];
                Vector2 screenPos = NormalizedToCanvas(normalized);

                markers[joint].gameObject.SetActive(true);
                markers[joint].anchoredPosition = screenPos;
            }
            else
            {
                markers[joint].gameObject.SetActive(false);
            }
        }
    }

    Vector2 NormalizedToCanvas(Vector2 normalized)
    {
        float x = (normalized.x - 0.5f) * canvasRect.sizeDelta.x;
        float y = (0.5f - normalized.y) * canvasRect.sizeDelta.y;
        return new Vector2(x, y);
    }
}