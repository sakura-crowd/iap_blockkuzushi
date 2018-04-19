using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定されたサイズおよびアスペクト比で画面に表示するように、カメラを調節します。  
/// </summary>
[RequireComponent(typeof(UnityEngine.Camera))]
public class CameraStableAspect : MonoBehaviour {

    private Camera cam;
    // 画面のサイズ（ピクセル単位）
    public float width = 540f;
    public float height = 960f;
    // 画像のPixel Per Unit
    public float pixelPerUnit = 100f;

    // 最後に調整したときの入力サイズを保持しておき、リサイズされたことを検知できるようにします。  
    protected float lastWidth = 540f;
    protected float lastHeight = 960f;
    protected float lastScreenWidth = 0f;
    protected float lastScreenHeight = 0f;

    void Awake()
    {
        this.lastWidth = this.width;
        this.lastHeight = this.height;
        Adjust();
    }

    private void Update()
    {
        // 設定サイズの変更または画面サイズの変更があれば調整する。
        if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height || lastWidth != width || lastHeight != height)
        {
            Adjust();
        }
    }

    /// <summary>
    /// 参照：Unity2Dで画面のアスペクト比を固定にしたい【Unity】 - Qiita : http://qiita.com/kwst/items/371542a6d3892b577b41
    /// </summary>
    void Adjust()
    {
        float aspect = (float)Screen.height / (float)Screen.width;
        float bgAcpect = height / width;

        // カメラコンポーネントを取得します
        cam = GetComponent<Camera>();

        if (this.width < this.height)   // 縦長
        {
            // カメラのorthographicSizeを設定
            cam.orthographicSize = (height / 2f / pixelPerUnit);
        }
        else // 横長
        {
            // カメラのorthographicSizeを設定
            cam.orthographicSize = (width / 2f / pixelPerUnit);
        }

        if (bgAcpect > aspect)
        {
            // 倍率
            float bgScale = height / Screen.height;
            // viewport rectの幅
            float camWidth = width / (Screen.width * bgScale);
            // viewportRectを設定
            cam.rect = new Rect((1f - camWidth) / 2f, 0f, camWidth, 1f);
        }
        else
        {
            // 倍率
            float bgScale = width / Screen.width;
            // viewport rectの幅
            float camHeight = height / (Screen.height * bgScale);
            // viewportRectを設定
            cam.rect = new Rect(0f, (1f - camHeight) / 2f, 1f, camHeight);
        }

        // 追加
        lastWidth = width;
        lastHeight = height;
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
}
