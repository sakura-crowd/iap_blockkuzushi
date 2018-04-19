using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁の制御。
/// </summary>
public class Wall : MonoBehaviour
{
    /// <summary>
    /// ボールが壁にぶつかったときの効果音  
    /// </summary>
    public AudioClip seHitWall;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ボールが衝突したときに、効果音を再生します。
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            // 効果音を再生
            Util.PlayAudioClip(this.seHitWall, Camera.main.transform.position, 1.0f);
        }
    }
}
