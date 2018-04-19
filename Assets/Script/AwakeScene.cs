using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1度だけ行う初期化処理のためのシーン Awake の制御。  
/// シーン上の AppC コンポーネントなどの初期化が完了すると、すぐに次のシーン Title へ移行する。
/// </summary>
public class AwakeScene : MonoBehaviour {

    /// <summary>
    /// 次のシーンの名前
    /// </summary>
    public string nameNextScene = "Title";

	// Use this for initialization
	void Start () {
        UnityEngine.SceneManagement.SceneManager.LoadScene(this.nameNextScene);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
