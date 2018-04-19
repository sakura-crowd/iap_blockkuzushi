using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム固有のアプリ内課金の処理を行います。
/// 
/// シーンが切り替わっても消えません。実行中に1回だけ作成してください。 
/// AppC コンポーネントの Sender に、このコンポーネントを含むオブジェクトを設定してください。
/// 
/// ▼ AppC のイベントのリレー
/// このオブジェクトを AppC コンポーネントの Sender に設定することで
/// AppC からの itemstore に関するイベントを、他のコンポーネントにリレーできます。
/// 
/// 例えば、 itemstore のショップ画面を閉じた直後にアイテム数を更新したい場合は
/// ショップ画面を閉じたイベントを受け取る必要があります。
/// 
/// その場合は、
/// ItemstoreClient.instance.callbackClosedItemstoreView += イベント関数;
/// で、任意のイベント関数がショップ画面を閉じた直後に呼び出されます。
/// </summary>
public class ItemstoreClient : MonoBehaviour {

    /// <summary>
    /// 唯一のインスタンス。
    /// </summary>
    public static ItemstoreClient instance = null;

    /// <summary>
    /// itemstore に登録したコンティニューに必要な消費型課金アイテムのグループID。
    /// ゲーム固有の変数。
    /// </summary>
    public const string GROUP_ID_KAKIN_ITEM = "kakinitem";

    /// <summary>
    /// 課金アイテム（コンティニュー用）の所持数。
    /// ゲーム固有の変数。
    /// </summary>
    public int cntKakinItem = 0;

    /// <summary>
    /// FinishedSetupAppC イベントのデリゲート
    /// </summary>
    /// <param name="b">Callback result.</param>
    public delegate void CallBackFinishedSetupAppC(bool b);

    /// <summary>
    /// AppC の FinishedSetupAppC（itemstore 機能の初期化が完了したとき）のイベントです。
    ///  +=, -= で同じ関数型をコールバックとして登録・登録解除できます。
    /// </summary>
    public event CallBackFinishedSetupAppC callbackFinishedSetupAppC;

    /// <summary>
    /// ClosedItemstoreView イベントのデリゲート
    /// </summary>
    public delegate void CallBackClosedItemstoreView();

    /// <summary>
    /// AppC の ClosedItemstoreView（itemstore のショップ画面が閉じたとき）のイベントです。
    ///  +=, -= で同じ関数型をコールバックとして登録・登録解除できます。
    /// </summary>
    public event CallBackClosedItemstoreView callbackClosedItemstoreView;

    void Awake()
    {
        if (instance != null)
        {
            string msg = "すでに ItemstoreClient は作成されています。";
            Debug.LogWarning(msg);
            throw new System.Exception(msg);
        }
        // 最初に作成されたものだけが設定されます。
        instance = GetComponent<ItemstoreClient>();
        // シーンが切り替わっても消えないように設定します。
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // AppC のイベント (AppC にこのコンポーネントを含むオブジェクトを登録すること）

    /// <summary>
    /// Finished setup callback reciever.
    /// </summary>
    /// <param name="b">Callback result.</param>
    public void FinishedSetupAppC(bool b)
    {
        Debug.Log("(Client)*** AppC FinishedSetupAppC ***");
        Debug.Log("(Client)AppC Initialize " + (b ? "Succeed!" : "Failure..."));

        // 情報を更新する
        UpdateKakinItemCount();

        // 登録されているコールバックを全て呼び出す。
        if (callbackFinishedSetupAppC != null)
        {
            callbackFinishedSetupAppC(b);
        }
    }

    /// <summary>
    /// Default push parameter reciever.
    /// </summary>
    /// <param name="str">Push custom parameter.</param>
    public void PushParameter(string str)
    {
        Debug.Log("(Client)PushParameter:" + str);
    }

    /// <summary>
    /// itemstore のショップ画面が閉じられたときに AppC から呼び出される関数です。
    /// AppC の Sender にオブジェクトを登録していない場合は、呼び出されません。
    /// Closed itemstore view callback reciever.
    /// </summary>
    public void ClosedItemstoreView()
    {
        Debug.Log("(Client)*** AppC ClosedItemstoreView ***");

        // 情報を更新する
        UpdateKakinItemCount();

        // 登録されているコールバックを全て呼び出す。
        if (callbackClosedItemstoreView != null)
        {
            callbackClosedItemstoreView();
        }
    }

    /// <summary>
    /// Recover reset callback reciever.
    /// </summary>
    public void RecoverReset()
    {
        Debug.Log("(Client)*** AppC RecoverReset ***");
    }

    /// <summary>
    /// Recover restored callback reciever.
    /// </summary>
    public void RecoverRestored()
    {
        Debug.Log("(Client)*** AppC RecoverRestored ***");
    }


    // ゲーム独自の関数

    /// <summary>
    /// 課金アイテム（コンティニュー用）の数を更新します。
    /// この数値は保持されており GetKakinItemCount() で取得できます。 
    /// 
    /// 次のイベント内でも更新をしています。コールバックがある場合、それを呼ぶ前に更新しています。
    ///   - FinishedSetupAppC
    ///   - ClosedItemstoreView
    /// また、 UseKakinItem でも１減らしたあと更新しています。
    /// </summary>
    public void UpdateKakinItemCount()
    {
        // 消費型アイテム
        // [itemstore管理画面] -> [アプリ管理] -> [アイテム管理] で設定したグループIDを指定し、アイテム数を取得する 
        this.cntKakinItem = AppC.ItemStore.GetItemCount(GROUP_ID_KAKIN_ITEM);
    }

    /// <summary>
    /// 最後に更新した際の課金アイテム（コンティニュー用）の数を返します。
    /// 課金アイテム（コンティニュー用）の数は UpdateKakinItemCount() で最新の情報に更新されます。
    /// </summary>
    /// <returns></returns>
    public int GetKakinItemCount()
    {
        return this.cntKakinItem;
    }

    /// <summary>
    /// 課金アイテム（コンティニュー用）を１個消費します。
    /// </summary>
    /// <returns></returns>
    public void UseKakinItem()
    {
        // Itemstore の課金アイテム（コンティニュー用）の数を１減らします。
        AppC.ItemStore.AddItemCount(GROUP_ID_KAKIN_ITEM, -1);
        // 課金アイテム（コンティニュー用）の数の更新を更新します。
        UpdateKakinItemCount();
    }

}
