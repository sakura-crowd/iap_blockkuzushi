using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームオーバー領域の制御。
/// </summary>
public class GameoverArea : MonoBehaviour {

    /// <summary>
    /// ゲームオーバー処理の関数を持つコンポーネントを設定しておきます。
    /// </summary>
    public StageCommonScene stageCommon;

	// Use this for initialization
	void Start () {
		if (stageCommon == null)
        {
            Debug.LogWarning("stageCommon が設定されていません。");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ボールとの接触イベントです。
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ball と接触したらゲームオーバー処理を呼び出します。 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            stageCommon.OnGameover();
        }
    }
}
