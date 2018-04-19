using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バーの制御。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bar : MonoBehaviour {

    /// <summary>
    /// ボールがぶつかったときの効果音 
    /// </summary>
    public AudioClip seHit;

    /// <summary>
    /// 1秒間で横に移動する最大量です。
    /// </summary>
    public float limitMovePerSec = 30f / 64f;

    /// <summary>
    /// 最後にタッチされた位置です。 this.flgMove と併用します。
    /// </summary>
    private Vector3 wPositionLastTouch = Vector3.zero;
    /// <summary>
    /// wPositionLastTouch の入力に応じて FixedUpdate の移動処理をする際に true になります。
    /// FixedUpdate で移動処理を行うと false に戻されます。
    /// </summary>
    private bool flgMove = true;

    /// <summary>
    /// ポーズ中は true となりボールが移動しない。
    /// </summary>
    bool flgPause = false;

    /// <summary>
    /// キャッシュ変数。
    /// </summary>
    private Rigidbody2D rb = null;
    /// <summary>
    /// キャッシュ変数。
    /// </summary>
    private Collider2D col = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
    /// <summary>
    /// 入力があれば、次回の FixedUpdate で移動するように入力座標を変数に設定します。
    /// </summary>
	// Update is called once per frame
	void Update () {

        // タッチされている / マウス左ボタンが押されている
        if (Input.GetMouseButton(0))
        {
            // タッチ/マウス左ボタンダウンしている位置をワールド座標に変換 
            this.wPositionLastTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 移動を指示するフラグをセットする。
            this.flgMove = true;
        }
    }

    /// <summary>
    /// 入力に応じてバーを移動させます。
    /// </summary>
    private void FixedUpdate()
    {
        // ポーズ中は移動しない。
        if (flgPause == true)
        {
            return;
        }

        // 一度加速度を 0 にする。
        rb.velocity = Vector2.zero;

        // 移動の入力があった場合
        if (this.flgMove == true)
        {
            // 移動する方向は、タッチしている方向にします。
            // 移動量の上限で丸めて、上限の移動速度を越えないようにします。
            Vector2 vecMove = new Vector2(
                Mathf.Clamp(this.wPositionLastTouch.x - this.transform.position.x, -this.limitMovePerSec, +this.limitMovePerSec),
                0f);
            // 現在の位置に移動量を足して、移動先を決めます。
            Vector2 vecPosition = new Vector2(vecMove.x + this.transform.position.x, vecMove.y + this.transform.position.y);

            // 当たり判定を行いながら指定した量だけ移動します
            rb.MovePosition(vecPosition);

            // 直前の Update の指示どおり移動したのでフラグを元に戻します
            this.flgMove = false;
        }
    }

    /// <summary>
    /// ボールと衝突した際に、バーの当たった位置に応じてボールの角度の変更を行います。
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            
            // 接触した x 座標が、バーの中心を基準にどれほど離れているか距離を求めます。 
            // その距離をバーの横幅で割り 0 ~ 1.0 の比率にします。
            // その比率に 45f をかけ、　90 度を基準として 90 + 45 ～ 90 - 45 度を設定します。 
            // 最初に - しているのは、左側が 90 + 45 というように + にさせるためです。座標は右が + だから反転させます。
            // bounds.size.x / 2 は中心からの距離に対しての割り算なので半分にする必要があります。
            float deg = - (collision.contacts[0].point.x - col.bounds.center.x) / (col.bounds.size.x /2) * 45f + 90f;

            // ボールの移動する方向を変更します。
            Ball compBall = collision.gameObject.GetComponent<Ball>();
            compBall.ChangeDirection(deg);

            // 効果音を再生します。
            Util.PlayAudioClip(this.seHit, Camera.main.transform.position, 1.0f);
        }
    }

    /// <summary>
    /// ポーズ状態にします。ポーズ中は入力を受け付けません。
    /// </summary>
    /// <param name="flg">true でポーズ状態にします。 false で解除します。</param>
    public void ChangePauseFlag(bool flg)
    {
        this.flgPause = flg;
    }
}
