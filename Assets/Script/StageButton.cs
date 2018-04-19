using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ選択ボタンの制御。
/// 
/// 状態に応じてボタンの見た目を変化させる必要があるので、独自のスクリプトでボタンを制御します。 
/// 外部インスペクターへの登録が必要ないため、プレハブをシーンに配置するだけで簡単にステージ選択ボタンを増やすことができます。
/// </summary>
public class StageButton : MonoBehaviour {

    /// <summary>
    /// PlayerPrefs に保存しているステージの状態を問い合わせるキーの接頭辞。
    /// この接頭辞のあとにステージのシーン名を組み合わせてキーとします。
    /// </summary>
    public const string prefixKeyStageStatus = "StageStatus_";

    /// <summary>
    /// ボタンが押されたときの効果音
    /// </summary>
    public AudioClip seButton;

    // ステージボタンの状態
    public enum StageStatus
    {
        // 選択不可
        Locked = 0,
        // 選択可能
        Unlocked = 1,
        // クリア済み
        Cleared = 2
    }

    /// <summary>
    /// 最初のステージであることを指定するためのフラグです。
    /// 
    /// 最初のステージの場合のみインスペクターで true にしてください。
    /// この値が true ならば、ステージボタンの状態が Locked(選択不可)の場合も選択可能になります。
    /// </summary>
    public bool flgFirstStage;

    /// <summary>
    /// このステージ選択ボタンが押されたときにロードするステージのシーン名です。
    /// ステージのシーン名でクリア状況は管理されています。
    /// </summary>
    public string nameStage;

    /// <summary>
    /// クリア時のボタンの色
    /// </summary>
    public Color colorOnClear = new Color(255, 182, 193);   // Lightpink

    /// <summary>
    /// ロック状態を表現する画像のオブジェクト。
    /// Hierarchy でボタンオブジェクトの下位においてください。
    /// </summary>
    public GameObject imageLocked = null;

    void Awake()
    {
        // ステージの状態を管理する PlayerPrefs のキーを作成します。
        // 接頭辞にステージのシーン名を足した文字列がキーになります。
        string keyStageStatus = prefixKeyStageStatus + nameStage;

        // ボタンのコンポーネントを取得します。
        UnityEngine.UI.Button coButton = GetComponent<UnityEngine.UI.Button>();

        // ステージの状態の記録がない場合は新規で設定します。
        // インストール直後のゲームの起動の際に行われます。
        if (PlayerPrefs.HasKey(keyStageStatus) == false)
        {
            // 最初のステージだけを「選択可能」にして、他は「選択不可」にします。
            if (flgFirstStage == true)
            {
                PlayerPrefs.SetInt(keyStageStatus, (int)StageStatus.Unlocked);
            }
            else
            {
                PlayerPrefs.SetInt(keyStageStatus, (int)StageStatus.Locked);
            }
        }

        // ステージの状態を取得します
        int stageStatus = PlayerPrefs.GetInt(keyStageStatus);

        // ステージの状態がクリア済みの場合
        if (stageStatus == (int)StageStatus.Cleared)
        {
            // すでにクリアしている場合は色をクリア済みのものに変えます
            UnityEngine.UI.ColorBlock colorBlock = coButton.colors;
            colorBlock.normalColor = colorOnClear;
            colorBlock.highlightedColor = colorOnClear;
            coButton.colors = colorBlock;
        }

        // ステージの状態が選択不可の場合
        if (stageStatus == (int)StageStatus.Locked)
        {
            // ロック解除されていない
            if (flgFirstStage == true)
            {
                // ただし、最初のステージのボタンは、強制的にロックを解除する
            }
            else
            {
                // ボタンコンポーネントを押せないように無効にする
                coButton.enabled = false;
                // ロック状態を表す画像のオブジェクトを有効にする
                imageLocked.SetActive(true);
            }
        }
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// ボタンが押されたときのイベント
    /// タイトルシーンから選択されたステージシーンへ画面を遷移します。
    /// </summary>
    public void OnButtonStage()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // シーンを読み込みます。
        // 最初に Stage1, Stage2, Stage3 のようにブロックを配置したステージ固有のシーンを読み込みます。
        UnityEngine.SceneManagement.SceneManager.LoadScene(this.nameStage);
        // その後、バー、ボール、壁などの全てのステージで共通のシーンを追加で読み込みます。
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageCommon", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
