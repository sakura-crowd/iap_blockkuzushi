using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイするステージの共通処理。　
/// </summary>
public class StageCommonScene : MonoBehaviour {

    /// <summary>
    /// 崩す対象のブロックを Hierarchy の下位に配置したオブジェクトです。  
    /// このオブジェクトの下位のオブジェクト（ブロック）の個数が 0 個になるとステージクリアです。
    /// 
    /// ステージ固有シーンに配置された StageCommonConfig コンポーネント経由でこのシーンのロード時に設定されます。
    /// </summary>
    public GameObject blocks;

    /// <summary>
    /// 次のステージ固有のシーン名です。
    /// 例えば Stage1 の次が Stage2 の場合は、 Stage1 のこのフィールドに Stage2 を設定します。
    /// 最終ステージの場合は空文字列を設定してください。
    /// 空文字列の場合は次のステージへのボタン表示や「使用可能」へのステータス変更を行いません。   
    /// 
    /// ステージ固有シーンに配置された StageCommonConfig コンポーネント経由でこのシーンのロード時に設定されます。
    /// </summary>
    public string nameNextStage;

    /// <summary>
    /// ステージクリア時のパネル
    /// </summary>
    public GameObject panelOnStageComplete;

    /// <summary>
    /// ゲームオーバー時のパネル
    /// </summary>
    public GameObject panelOnGameover;

    /// <summary>
    /// コンティニューボタン。アイテムがあればボタン有効。
    /// </summary>
    public UnityEngine.UI.Button buttonContinue;

    /// <summary>
    /// 課金アイテム（コンティニュー用）の残りの数。
    /// </summary>
    public UnityEngine.UI.Text uiTextKakinItemCount;

    /// <summary>
    /// デバッグ用。 true の場合は、アイテムの個数が 0 でもコンティニューボタンを有効にします。
    /// </summary>
    public bool flgDebugContinueAlways = false;

    /// <summary>
    /// ステージクリア時の[Next Stage]ボタンです。
    /// パネルなどへの拡張も考えて GameObject で受け取ります。
    /// </summary>
    public GameObject goNextStage;

    /// <summary>
    /// ボールのコンポーネントをインスペクターから設定してください。
    /// プレイ開始直後に移動を開始させたり、プレイ終了後に移動を停止します。
    /// </summary>
    public Ball coBall;

    /// <summary>
    /// バーのコンポーネントをインスペクターから設定してください。
    /// プレイ開始直後に移動を開始させたり、プレイ終了後に移動を停止します。
    /// </summary>
    public Bar coBar;

    /// <summary>
    /// ステージ開始直後のポーズ時のパネルをインスペクターから設定してください。
    /// ステージ開始直後に有効になっています。
    /// タッチされてポーズを解除した際に、非表示にします。
    /// </summary>
    public GameObject panelOnPause;

    /// <summary>
    /// ステージの処理の段階
    /// </summary>
    public enum StagePlayPhase
    {
        // プレイ開始前(ポーズ中)
        Before = 0,
        // プレイ中
        Playing = 1,
        // ステージクリア時
        StageComplete = 2,
        // ゲームオーバー時
        Gameover = 3
    }

    /// <summary>
    /// ステージの処理の段階を保持する変数です。
    /// </summary>
    StagePlayPhase phase = StagePlayPhase.Before;


    /// <summary>
    /// ステージクリアのときの効果音
    /// </summary>
    public AudioClip seStageComplete;

    /// <summary>
    /// ゲームオーバーのときの効果音
    /// </summary>
    public AudioClip seGameover;

    /// <summary>
    /// ボタンが押されたときの効果音
    /// </summary>
    public AudioClip seButton;

    void Awake()
    {
        // ポーズ時パネルを有効にする。
        this.panelOnPause.SetActive(true);
    }

    // Use this for initialization
    void Start () {
        // 設定の確認
        if (this.blocks == null)
        {
            Debug.LogWarning("StageCommonConfig オブジェクトに blocks が設定されていません。");
        }

        // itemstore のショップ画面が閉じたときのイベント(ClosedItemstoreView)を受け取れるようにイベントを登録します。
        if (ItemstoreClient.instance != null)
        {
            ItemstoreClient.instance.callbackFinishedSetupAppC += OnFinishedSetupAppC;
            ItemstoreClient.instance.callbackClosedItemstoreView += OnClosedItemstoreView;
        }
    }

    private void OnDestroy()
    {
        // Start() で登録したイベントを登録解除します。
        if (ItemstoreClient.instance != null)
        {
            ItemstoreClient.instance.callbackFinishedSetupAppC -= OnFinishedSetupAppC;
            ItemstoreClient.instance.callbackClosedItemstoreView -= OnClosedItemstoreView;
        }
    }

    /// <summary>
    /// AppC コンポーネントの itemstore の機能が初期化されたときのイベントです。
    /// ItemstoreClient を経由して AppC の FinishedSetupAppC イベントの際に呼び出されます。 
    /// <param name="b">Callback result.</param>
    /// </summary>
    public void OnFinishedSetupAppC(bool b)
    {
        // アイテムの所持数を更新します。
        UpdateKakinItemUI();
    }

    /// <summary>
    /// itemstore のショップ画面が閉じたときのイベントです。
    /// ショップ画面で購入した可能性があるので、アイテムの個数を更新します。
    /// </summary>
    public void OnClosedItemstoreView()
    {
        // 課金アイテム（コンティニュー用）の所持数とコンティニューボタンを更新
        UpdateKakinItemUI();
    }

    // Update is called once per frame
    void Update () {
        // 崩す対象のブロックがなくなったら、ステージクリアの処理を呼び出します。
        if (blocks.transform.childCount == 0)
        {
            OnStageComplete();
        }
	}

    /// <summary>
    /// ポーズ中に表示されているパネルにタッチした直後のイベント。
    /// </summary>
    public void OnPausePanelPointerDown()
    {
        // タッチされた座標をスクリーン座標からワールド座標に変換します。
        // ボールやバーの位置をワールド座標で扱っているため、同じくワールド座標にします。
        Vector3 wPointerDownOnPause = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ステージ開始直後、プレイ前のポーズ中の場合
        if (this.phase == StagePlayPhase.Before)
        {
            // ゲーム開始時のみタップした方向にボールの移動方向を設定します。
            // Vector2.Angle は from - (0, 0) - to の成す角度を計算するので、目標位置をボールの位置からひきます。
            float deg = Vector2.Angle(Vector2.right, wPointerDownOnPause - coBall.gameObject.transform.position);
            this.coBall.ChangeDirection(deg);
            // フェーズをプレイ中に変更します
            this.phase = StagePlayPhase.Playing;
        }

        // ボールのコンポーネントのポーズ状態を解除します。 
        this.coBall.ChangePauseFlag(false);
        // バーのポーズ状態を解除します。
        coBar.ChangePauseFlag(false);
        // ポーズ時のパネルを無効、非表示にします。
        this.panelOnPause.SetActive(false);
    }

    /// <summary>
    /// ステージクリア時の処理です。
    /// </summary>
    public void OnStageComplete()
    {
        // すでにゲームをクリアしているかゲームオーバーならば何もしません。
        if ((int)this.phase > (int)StagePlayPhase.Playing)
        {
            return;
        }

        // フェーズをプレイ中からステージクリア時に変更します。
        this.phase = StagePlayPhase.StageComplete;

        // ボールを停止させます。
        coBall.ChangePauseFlag(true);
        // バーを停止させます。
        coBar.ChangePauseFlag(true);

        // 効果音を再生
        Util.PlayAudioClip(this.seStageComplete, Camera.main.transform.position, 1.0f);

        // 現在のステージの状態を「クリア済み」にします。
        string keyStageStatus = StageButton.prefixKeyStageStatus + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(keyStageStatus, (int)StageButton.StageStatus.Cleared);

        // 次のステージが設定されている場合
        if (string.IsNullOrEmpty(this.nameNextStage) == false)
        {
            // 次のステージのステータスが「選択不可」ならば「選択可能」にします。
            string keyNextStageStatus = StageButton.prefixKeyStageStatus + this.nameNextStage;
            if (PlayerPrefs.GetInt(keyNextStageStatus) == (int)StageButton.StageStatus.Locked)
            {
                PlayerPrefs.SetInt(keyNextStageStatus, (int)StageButton.StageStatus.Unlocked);
            }
        }
        else
        {
            // 次のステージがない場合は、[Next Stage]の UI は無効、非表示にします。
            this.goNextStage.SetActive(false);
        }

        // ステージクリア時のパネルを有効にします。
        panelOnStageComplete.SetActive(true);
    }

    /// <summary>
    /// ゲームオーバー時の処理です。
    /// </summary>
    public void OnGameover()
    {
        // すでにゲームをクリアしているかゲームオーバーならば何もしません。
        if ((int)this.phase > (int)StagePlayPhase.Playing)
        {
            return;
        }

        // フェーズをプレイ中からゲームオーバー時に変更します。
        this.phase = StagePlayPhase.Gameover;

        // ボールを停止させます。
        coBall.ChangePauseFlag(true);
        // バーを停止させます。
        coBar.ChangePauseFlag(true);

        // 効果音を再生
        Util.PlayAudioClip(this.seGameover, Camera.main.transform.position, 1.0f);

        // ステージクリア時のパネルを有効にします。 
        panelOnGameover.SetActive(true);
        // 課金アイテム（コンティニュー用）の所持数とコンティニューボタンを更新します。
        UpdateKakinItemUI();
    }

    /// <summary>
    /// 課金アイテム（コンティニュー用）の所持数、コンティニューボタンの状態を更新します。 
    /// </summary>
    public void UpdateKakinItemUI()
    {
        // 課金アイテム（コンティニュー用）の所持数を取得します。
        int cnt = ItemstoreClient.instance.GetKakinItemCount();
        // 所持数の表示を更新します。
        this.uiTextKakinItemCount.text = string.Format("x{0}", cnt);
        // 課金アイテム（コンティニュー用）がなければ、コンティニューボタンを無効にします。
        // ただし、デバッグ用のフラグが設定されている場合は常に有効にします。
        if (this.flgDebugContinueAlways == false && cnt <= 0)
        {
            // enabled = false でも押せなくなりますが、見た目が変わりません。
            // interactable = false では押せなくなると同時に見た目も変わります。
            this.buttonContinue.interactable = false;
        }
        else
        {
            // 例えば、ゲームオーバー直後は 0 個で無効だったけれども
            // ショップで購入した場合などに、ここでコンティニューボタンを有効にします。
            this.buttonContinue.interactable = true;
        }
    }

    /// <summary>
    /// 「ショップ」ボタンの押下イベントです。
    /// </summary>
    public void OnButtonShop()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // itemstoreビューを開きます。
        AppC.ItemStore.OpenItemStoreView();
    }

    /// <summary>
    /// 「リトライ」ボタンの押下イベントです。
    /// </summary>
    public void OnButtonRetry()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // 現在のステージを読み込みます。
        // ステージ選択ボタンの押下イベントと同じように、
        // ステージ固有シーンをロードしてから、ステージ共通シーンを追加でロードします。
        string nameStage = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameStage);
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageCommon", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    /// <summary>
    /// 「タイトル」ボタンの押下イベントです。
    /// </summary>
    public void OnButtonTitle()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // タイトルのシーンをロードします。
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// 「次のステージ」ボタンの押下イベントです。
    /// </summary>
    public void OnButtonNextStage()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);

        // 次のステージが設定してある場合
        if (string.IsNullOrEmpty(nameNextStage) != true)
        {
            // 設定してある次のステージをロードします。
            UnityEngine.SceneManagement.SceneManager.LoadScene(nameNextStage);
            // ステージ共通シーンを追加でロードします。
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageCommon", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// 「コンティニュー」ボタンの押下イベントです。
    /// </summary>
    public void OnButtonContinue()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);

        // ステージ共通シーンをリセットします。
        // ここではステージ共通シーンを Unload し、それが完了したイベントで再びロードする処理を行います。　
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnUnloadedSceneForContinue;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("StageCommon");

        // 課金アイテム（コンティニュー用）の数を１個減らす
        ItemstoreClient.instance.UseKakinItem();
    }

    /// <summary>
    /// コンティニュー時にステージ共通シーンが Unload されたときのイベントです。
    /// Unload の完了を待たずに Load すると、エディタ実行時にフリーズしたので、完了を待ってからロードします。
    /// </summary>
    /// <param name="scene"></param>
    public void OnUnloadedSceneForContinue(UnityEngine.SceneManagement.Scene scene)
    {
        // Unload されたシーンの名前が StageCommon の場合
        if (scene.name == "StageCommon")
        {
            // さきほど登録した SceneUnload のイベントを登録解除します。
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnUnloadedSceneForContinue;
            // 追加で StageCommon を再びロードします。
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageCommon", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }
}

