using System;
using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient Instance;
    
    [Header("Server Settings")]
    public string serverUrl = "ws://localhost:8765";
    
    private WebSocket websocket;
    public Dictionary<string, Vector2> JointPositions = new Dictionary<string, Vector2>();
    public bool IsConnected => websocket?.State == WebSocketState.Open;

    void Awake()
    {
        Instance = this;
    }

    async void Start()
    {
        await Connect();
    }

    async System.Threading.Tasks.Task Connect()
    {
        websocket = new WebSocket(serverUrl);

        websocket.OnOpen += () => Debug.Log("WebSocket Connected!");
        websocket.OnError += (e) => Debug.LogError("WebSocket Error: " + e);
        websocket.OnClose += (e) => Debug.Log("WebSocket Closed");
        websocket.OnMessage += (bytes) =>
        {
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            ParseJoints(json);
        };

        await websocket.Connect();
    }

    void ParseJoints(string json)
    {
        try
        {
            JointData data = JsonUtility.FromJson<JointData>(json);
            JointPositions.Clear();
            foreach (var joint in data.joints)
            {
                JointPositions[joint.name] = new Vector2(joint.x, joint.y);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Parse error: " + e.Message);
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    async void OnDestroy()
    {
        if (websocket != null)
            await websocket.Close();
    }
}

[Serializable]
public class JointData
{
    public JointEntry[] joints;
}

[Serializable]
public class JointEntry
{
    public string name;
    public float x;
    public float y;
}