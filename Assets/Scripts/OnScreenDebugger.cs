using UnityEngine;
using System.Collections.Generic;

// Found at: https://gist.github.com/AppleBoiy/958a5b47891e2c789ab0e5e4e5c109c2
public class OnScreenDebugger : MonoBehaviour
{
    private readonly Queue<string> _logQueue = new Queue<string>();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedLog = "[" + type + "] : " + logString;
        _logQueue.Enqueue(formattedLog);
        if (type == LogType.Exception)
        {
            _logQueue.Enqueue(stackTrace);
        }
        while (_logQueue.Count > 20) // Keep the log queue at a manageable size
        {
            _logQueue.Dequeue();
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        GUILayout.Label("\n" + string.Join("\n", _logQueue.ToArray()));
        GUILayout.EndArea();
    }
}