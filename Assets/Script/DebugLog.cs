using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 【Unity】ゲーム画面にDebug.Logを出したい！ - うら干物書き : http://www.urablog.xyz/entry/2017/04/25/195351
/// Debug.Log を出力してくれる。 
/// </summary>
public class DebugLog : MonoBehaviour {

    public UnityEngine.UI.Text uiText;

    private void Awake()
    {
        Application.logMessageReceived += OnLogMessage;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessage;
    }

    private void OnLogMessage(string i_logText, string i_stackTrace, LogType i_type)
    {
        if (string.IsNullOrEmpty(i_logText))
        {
            return;
        }

        uiText.text += i_logText + System.Environment.NewLine;
    }
}
