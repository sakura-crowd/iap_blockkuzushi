using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージの共通処理で必要な、各ステージ固有の情報を管理します。
/// 
/// 各ステージ(例えば Stage1, Stage2, Stage3)に必ず１つずつ配置してください。
/// ステージ共通の処理で必要なデータをインスペクターで設定してください。
/// 
/// 例えば、ステージごとに異なる、次のステージのシーン名などを設定します。
/// これらの設定は、ステージ共通シーンの初期化で伝えられます。
/// </summary>
public class StageCommonConfig : MonoBehaviour {

    /// <summary>
    /// 崩す対象のブロックを Hierarchy の下位に配置したオブジェクトです。
    /// このオブジェクトの下位のオブジェクト（ブロック）の個数が 0 個になるとステージクリアです。
    /// </summary>
    public GameObject blocks;
    /// <summary>
    /// 次のステージ固有のシーン名です。
    /// 例えば Stage1 の次が Stage2 の場合は、 Stage1 のこのフィールドに Stage2 を設定します。
    /// 最終ステージの場合は空文字列を設定してください。
    /// 空文字列の場合は次のステージへのボタン表示や「使用可能」へのステータス変更を行いません。   
    /// </summary>
    public string nameNextStage;


    /// <summary>
    /// ステージ固有シーン(Stage1など)の Awake で、シーンがロードされたときのイベントに登録します。
    /// これで何かシーンがロードされたときに、OnLoadedSceneForContinue が呼び出されるようになります。
    /// </summary>
    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLoadedSceneForContinue;
    }
    /// <summary>
    /// ステージ固有シーン(Stage1など)が破棄されるときは、登録しておいたイベントを解除します。
    /// </summary>
    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLoadedSceneForContinue;
    }

    /// <summary>
    /// ステージ共通シーン StageCommon に配置されている StageCommonScene コンポーネントに、このステージ固有の情報を設定します。
    /// このイベントは、ステージ開始直後のほかに、コンティニューで StageCommon のみ再読み込みされたときも呼び出されます。
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void OnLoadedSceneForContinue(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // ロードされたシーンが StageCommon の場合
        if (scene.name == "StageCommon")
        {
            // StageCommon シーンに配置されている、特定のオブジェクトを探し、その中のコンポーネントに共通処理で使う設定をコピーします。
            foreach (GameObject rootObj in scene.GetRootGameObjects())
            {
                // StageCommon という名前のオブジェクトが持つ StageCommonScene コンポーネントに設定をコピーします。
                if (rootObj.name == "StageCommon")
                {
                    StageCommonScene coStageCommon = rootObj.GetComponent<StageCommonScene>();
                    if (coStageCommon != null)
                    {
                        coStageCommon.blocks = blocks;
                        coStageCommon.nameNextStage = nameNextStage;
                    }
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
		
	}
}
