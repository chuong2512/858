using System;
using System.IO;
using UnityEngine;

public class ExceptionHandler : MonoBehaviour
{
    private StreamWriter m_Writer;
    private int m_ExceptionCount = 0;

    void Awake()
    {
        Application.logMessageReceived += HandleException;
        m_Writer = new StreamWriter(Path.Combine(Application.persistentDataPath, "unityexceptions.txt"));
        m_Writer.AutoFlush = true;
    }

    private void HandleException(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            m_ExceptionCount++;
            m_Writer.WriteLine("{0}: {1}\n{2}", type, condition, stackTrace);
        }
    }

    void OnGUI()
    {
        GUILayout.Label(string.Format("Count: {0}", m_ExceptionCount));
        if (GUILayout.Button("Application Exception"))
        {
            throw new ApplicationException();
        }
        if (GUILayout.Button("Null Reference"))
        {
            GameObject go = null;
            Debug.Log(go.name);
        }
        if (GUILayout.Button("Float Divide By Zero"))
        {
            float x = 3.14f;
            float y = 0.0f;
            float z = x / y;
            Debug.Log(z.ToString());
        }
        if (GUILayout.Button("Integer Divide By Zero"))
        {
            int x = 42;
            int y = 0;
            int z = x / y;
            Debug.Log(z.ToString());
        }
        if (GUILayout.Button("Stack Overflow"))
        {
            OverflowStack(1, 2, 3);
        }
    }

    private int OverflowStack(int a, int b, int c)
    {
        return OverflowStack(c, b, a) + OverflowStack(b, c, a + 1);
    }
}