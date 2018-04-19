using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボールの制御。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Ball : MonoBehaviour {

    /// <summary>
    /// ボールの速度。大きいほど速く移動します。
    /// </summary>
    public float speedPerSec = 3000f;

    /// <summary>
    /// this.newDirection が有効な場合は true です。
    /// FixedUpdate で this.newDirection がボールの移動する方向に適用されると false に戻ります。
    /// </summary>
    bool flgChangeDirection = false;

    /// <summary>
    /// 外部から移動する方向を変更された場合に、その方向を次の FixedUpdate まで保持するためのベクトル変数。
    /// this.flgChangeDirection と併用します。
    /// </summary>
    Vector2 newDirection;

    /// <summary>
    /// ポーズ中は true となりボールが移動しません。
    /// 開始直後はポーズ状態になっています。
    /// </summary>
    bool flgPause = true;

    /// <summary>
    /// キャッシュ用の変数。
    /// </summary>
    Rigidbody2D rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 指示があれば、指定された角度に移動する方向を変更します。
    /// 水平、垂直に近い傾きになることを防ぐため、一定の角度に丸めます。
    /// </summary>
    private void FixedUpdate()
    {
        // ポーズ中は何もしません。
        if (this.flgPause == true)
        {
            return;
        }

        // 移動する方向のベクトルを正規化します。
        Vector2 velocityNormalized = rb.velocity.normalized;

        // 移動する方向を変える指示があった場合は、移動する方向のベクトルを指示したベクトルに書き換えます。
        if (flgChangeDirection == true)
        {
            velocityNormalized = newDirection;
            flgChangeDirection = false;
        }

        // 何もしないと角にぶつかったときに水平に移動を始める場合があります。
        // それではゲームに支障をきたすので、移動する方向が一定範囲の角度の場合は、許可される範囲に丸めます。
        float limitVerticalDeg = 10f;   // 垂直方向は 90 ± 10 度、270 ± 10 度の範囲の角度は近いほうに寄せる。
        float limitHorizontalDeg = 45f; // 水平方向は 0 ± 45 度、 180 ± 45 度の範囲の角度は近いほうに寄せる。
        if (velocityNormalized.x >= 0f)
        {
            velocityNormalized.x = Mathf.Clamp(velocityNormalized.x, Mathf.Cos(Mathf.Deg2Rad * (90 - limitVerticalDeg)), Mathf.Cos(Mathf.Deg2Rad * (0 + limitHorizontalDeg)));
        }
        else
        {
            velocityNormalized.x = Mathf.Clamp(velocityNormalized.x, Mathf.Cos(Mathf.Deg2Rad * (180 - limitHorizontalDeg)), Mathf.Cos(Mathf.Deg2Rad * (90 + limitVerticalDeg)));
        }
        if (velocityNormalized.y >= 0f)
        {
            velocityNormalized.y = Mathf.Clamp(velocityNormalized.y, Mathf.Sin(Mathf.Deg2Rad * (180 - limitHorizontalDeg)), Mathf.Sin(Mathf.Deg2Rad * (90 + limitVerticalDeg)));
        }
        else
        {
            velocityNormalized.y = Mathf.Clamp(velocityNormalized.y, Mathf.Sin(Mathf.Deg2Rad * (270 - limitVerticalDeg)), Mathf.Sin(Mathf.Deg2Rad * (180 + limitHorizontalDeg)));
        }

        // 調整された移動方向に、等速で移動するように加速度を設定します。
        rb.velocity = velocityNormalized.normalized * speedPerSec * Time.fixedDeltaTime;
    }

    /// <summary>
    /// 次の FixedUpdate で移動方向を変更します。
    /// </summary>
    /// <param name="deg"></param>
    public void ChangeDirection(float deg)
    {
        // 指定された角度は、正規化されたベクトルに変換して次の FixedUpdate まで保持します。
        newDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deg), Mathf.Sin(Mathf.Deg2Rad * deg));
        // 移動する方向を変更するためのフラグをセットします。
        flgChangeDirection = true;
    }

    /// <summary>
    /// ポーズ状態にする場合は true, ポーズ状態を解除する場合は false を設定します。
    /// </summary>
    /// <param name="flg"></param>
    public void ChangePauseFlag(bool flg)
    {
        this.flgPause = flg;
        if (flg == true)
        {
            // ポーズする場合は加速度を０にします。
            rb.velocity = Vector2.zero;
        }
    }
}
