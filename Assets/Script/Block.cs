using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブロックの制御。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Block : MonoBehaviour {

    /// <summary>
    /// ボールがぶつかったときの効果音です。
    /// 壊れるときは別の効果音です。
    /// </summary>
    public AudioClip seHit;

    /// <summary>
    /// ボールがぶつかって壊れるときの効果音です。
    /// </summary>
    public AudioClip seDestroy;

    /// <summary>
    /// ブロックの耐久力の初期値です。
    /// </summary>
    public int hpMax = 1;
    /// <summary>
    /// ブロックの耐久力です。最初に this.hpMax が代入されます。
    /// 0 以下になるとブロックは壊れます。
    /// </summary>
    public int hp = 0;

	// Use this for initialization
	void Start () {
        // ブロックの耐久力を初期化します。
        this.hp = hpMax;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ボールがぶつかったときのイベント処理です。
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトがボールの場合
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            // 耐久力を減らします。
            this.hp -= 1;

            // 壊れる場合
            if (this.hp <= 0)
            {
                // 壊されたときの効果音を再生
                Util.PlayAudioClip(this.seDestroy, Camera.main.transform.position, 1.0f);
                // このオブジェクトを破棄します。
                GameObject.Destroy(this.gameObject);
            }
            // まだ壊れない場合
            else
            {
                // 跳ね返す効果音を再生
                Util.PlayAudioClip(this.seHit, Camera.main.transform.position, 1.0f);
            }
        }
    }
}
