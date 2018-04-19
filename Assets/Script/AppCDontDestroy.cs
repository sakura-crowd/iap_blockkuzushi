using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AppC コンポーネントを持つオブジェクトにこのコンポーネントを付加するとシーンが切り替わっても消えません。 
/// </summary>
[RequireComponent(typeof(AppC))]
public class AppCDontDestroy : MonoBehaviour {

    /// <summary>
    /// 唯一の AppC オブジェクトを静的に保持します。
    /// </summary>
    static public AppC appc = null;

    void Awake()
    {
        if (appc != null)
        {
            Debug.LogWarning("AppC オブジェクトが2回以上作成されました。筆者はこの動作を確認していません。");
        }
        appc = GetComponent<AppC>();
        DontDestroyOnLoad(this.gameObject);
    }

}
