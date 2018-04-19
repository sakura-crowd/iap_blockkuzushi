using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトルシーンの制御。
/// </summary>
public class TitleScene : MonoBehaviour {

    /// <summary>
    /// ボタンが押されたときの効果音
    /// </summary>
    public AudioClip seButton;

    /// <summary>
    /// 課金アイテム（コンティニュー用）の数を表示するテキストをインスペクターから設定してください。
    /// </summary>
    public UnityEngine.UI.Text uiTextKakinItem = null;

    /// <summary>
    /// プライバシーポリシーを表示するパネルをインスペクターから設定してください。
    /// </summary>
    public GameObject uiPanelPrivacyPolicy = null;

    /// <summary>
    /// 宣伝用の URL です。
    /// この企画の連載記事の URL を設定しています。
    /// </summary>
    public string urlCM = "http://blog.item-store.net/entry/2017/12/14/101613";

    private void Awake()
    {
        // AppC の ClosedItemstoreView(itemstore のショップ画面が閉じたときの)イベントを登録します。
        if (ItemstoreClient.instance != null)
        {
            ItemstoreClient.instance.callbackFinishedSetupAppC += OnFinishedSetupAppC;
            ItemstoreClient.instance.callbackClosedItemstoreView += OnClosedItemstoreView;
        }
    }
    private void OnDestroy()
    {
        // Start で登録したイベントを登録解除します。
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
        UpdateUiTextKakinItemCount();
    }

    /// <summary>
    /// itemstore のショップ画面が閉じたときのイベントです。
    /// ItemstoreClient を経由して AppC の ClosedItemstoreView イベントの際に呼び出されます。 
    /// </summary>
    public void OnClosedItemstoreView()
    {
        // アイテムの所持数を更新します。
        UpdateUiTextKakinItemCount();
    }

    // Use this for initialization
    void Start () {
        // アイテムの所持数を更新します。
        UpdateUiTextKakinItemCount();
    }

    /// <summary>
    /// アイテムの所持数の表示を更新します。
    /// </summary>
    void UpdateUiTextKakinItemCount()
    {
        // 課金アイテム（コンティニュー用）の所持数を取得します。
        int cnt = ItemstoreClient.instance.GetKakinItemCount();
        // 課金アイテム（コンティニュー用）の所持数のテキストを更新します。
        uiTextKakinItem.text = string.Format("x{0}", cnt);
    }

    // Update is called once per frame
    void Update () {
		
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
    /// 「プライバシーポリシーについて」ボタンの押下イベントです。
    /// </summary>
    public void OnButtonPrivacyPolicy()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // プライバシーポリシーのパネルを有効にして表示します。
        this.uiPanelPrivacyPolicy.SetActive(true);
    }

    /// <summary>
    /// プライバシーポリシーを表示するパネルの「閉じる」ボタンの押下イベントです。
    /// </summary>
    public void OnButtonPrivacyPolicyClose()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // プライバシーポリシーのパネルを無効にします。
        this.uiPanelPrivacyPolicy.SetActive(false);
    }

    /// <summary>
    /// このアプリ開発の連載記事の宣伝用。
    /// </summary>
    public void OnButtonSiteUrlButton()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // 連載記事のサイトを開きます。
        Util.OpenURL(this.urlCM);
    }

    /// <summary>
    /// プレイヤー情報の初期化を行います。
    /// ステージの状態が初期化されます。
    /// デバッグ専用。
    /// </summary>
    public void OnDebugButtonPlayerPrefsClear()
    {
        // 効果音を再生
        Util.PlayAudioClip(this.seButton, Camera.main.transform.position, 1.0f);
        // このアプリの PlayerPrefs の設定を全て消去します。
        PlayerPrefs.DeleteAll();
    }
}
