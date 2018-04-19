using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// appC Unity Plugin
/// </summary>
public class AppC : MonoBehaviour
{
	/// <summary>
	/// API.
	/// </summary>
	public enum API
	{
		ITEMSTORE, PUSH, DATASTORE,
	}

	public static _ItemStore ItemStore;
	public static _Push Push;
	public static _Datastore Datastore;

	[Header("Media Key")]
	public string androidMediaKey;
	public string iOSMediaKey;
	[Space(1)]

	[Header("On API")]
	public bool itemStoreAPI;
	public bool pushAPI;
	public bool datastoreAPI;
	[Space(1)]

	[Header("Send CallBack Target (Default [This Object])")]
	public GameObject callbackTargetObject = null;
	[Space(10)]

	[Header("Other Settings (Android Only)")]
	[Header("Android Push Icon (Default [app_icon])")]
	public string iconName = "";

	[Header("Android Extends Activity (option)")]
	public string activityName = "";
	[Space(1)]

	public static bool? finishedInitFlag = null;
	private static List<API> apis = new List<API>();

	private static string param = "";

	/// <summary>
	/// On the specified api.
	/// </summary>
	/// <returns>The on.</returns>
	/// <param name="api">API.</param>
	public void On(API api)
	{
		apis.Add(api);
		switch (api)
		{
			case API.ITEMSTORE:
				Debug.Log("itemstore On");
				ItemStore = new _ItemStore();
				break;
			case API.PUSH:
				Debug.Log("Push On");
				Push = new _Push();
				break;
			case API.DATASTORE:
				Debug.Log("Datastore On");
				Datastore = new _Datastore();
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Callback.
	/// </summary>
	/// <param name="str">Callback result from native SDK.</param>
	public void OnCallBack(string str)
	{
		// str: "" + bool + "%INIT%" + getParam ()
		string[] strArrayData = str.Split(new string[] { "%INIT%" }, System.StringSplitOptions.None);

		// [0]：初期化結果
		Debug.Log("init result: " + strArrayData[0]);
		AppC.finishedInitFlag = bool.Parse(strArrayData[0]);

		// [1]：getParamの文字列
		Debug.Log("push message: " + strArrayData[1]);
		param = strArrayData[1];

		if (callbackTargetObject == null)
		{
			callbackTargetObject = this.gameObject;
		}
		callbackTargetObject.SendMessage("FinishedSetupAppC", AppC.finishedInitFlag);
		if (!string.IsNullOrEmpty(param))
		{
			callbackTargetObject.SendMessage("PushParameter", param);
		}
	}

	/// <summary>
	/// Finished setup callback reciever.
	/// </summary>
	/// <param name="b">Callback result.</param>
	public void FinishedSetupAppC(bool b)
	{
		Debug.Log("*** AppC FinishedSetupAppC ***");
		Debug.Log("AppC Initialize " + (b ? "Succeed!" : "Failure..."));
	}

	/// <summary>
	/// Default push parameter reciever.
	/// </summary>
	/// <param name="str">Push custom parameter.</param>
	public void PushParameter(string str)
	{
		Debug.Log("PushParameter:" + str);
	}

	/// <summary>
	/// Ons the call back closed itemstore view.
	/// </summary>
	public void OnCallBackClosedItemstoreView()
	{
		if (callbackTargetObject == null)
		{
			callbackTargetObject = this.gameObject;
		}
		callbackTargetObject.SendMessage("ClosedItemstoreView");
	}

	/// <summary>
	/// Closed itemstore view callback reciever.
	/// </summary>
	public void ClosedItemstoreView()
	{
		Debug.Log("*** AppC ClosedItemstoreView ***");
	}

	/// <summary>
	/// Ons the call back recover reset.
	/// </summary>
	public void OnCallBackRecoverReset()
	{
		if (callbackTargetObject == null)
		{
			callbackTargetObject = this.gameObject;
		}
		AppC.finishedInitFlag = true;
		callbackTargetObject.SendMessage("RecoverReset");
	}

	/// <summary>
	/// Recover reset callback reciever.
	/// </summary>
	public void RecoverReset()
	{
		Debug.Log("*** AppC RecoverReset ***");
	}

	/// <summary>
	/// Ons the call back recover restored.
	/// </summary>
	public void OnCallBackRecoverRestored()
	{
		if (callbackTargetObject == null)
		{
			callbackTargetObject = this.gameObject;
		}
		AppC.finishedInitFlag = true;
		callbackTargetObject.SendMessage("RecoverRestored");
	}

	/// <summary>
	/// Recover restored callback reciever.
	/// </summary>
	public void RecoverRestored()
	{
		Debug.Log("*** AppC RecoverRestored ***");
	}

	/// <summary>
	/// Check initialize.
	/// </summary>
	/// <returns>Initialize Result.</returns>
	public static bool IsFinished()
	{
		return AppC.finishedInitFlag.HasValue && (bool)AppC.finishedInitFlag;
	}

#if UNITY_ANDROID
    static AndroidJavaObject androidSDK = null;

    public void Init()
    {
        int[] apiNums = new int[apis.Count];
        for (int i = 0; i < apis.Count; i++)
        {
            apiNums[i] = (int)apis[i];
        }

        if (string.IsNullOrEmpty (iconName))
        {
            iconName = "app_icon";
        }
        
        if (string.IsNullOrEmpty (activityName))
        {
            activityName = "com.unity3d.player.UnityPlayerNativeActivity";
        }

        if (androidSDK != null)
        {
            androidSDK.Call ("InitAppC", this.name, (int[])apiNums, androidMediaKey, activityName, iconName);
        }
    }

    public static string GetInquiryKey ()
    {
        try
        {
            if (androidSDK != null)
            {
                return androidSDK.Call<string>("GetInquiryKey");
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    // Androidでアプリ終了の機能を実装している場合はPluginDestroyを呼び出してください（Androidのみ）
    public static void PluginDestroy ()
    {
        if (androidSDK != null)
        {
            androidSDK.Call ("Finish");
            Debug.Log ("AppC destroy");
        }
    }

    public static void OpenRecoverGenerateView ()
    {
        if (!AppC.IsFinished ())
        {
            return;
        }
        
        if (androidSDK != null)
        {
            androidSDK.Call("OpenGenerate");
        }
    }

    public static void OpenRecoverRestoreView ()
    {
        if (!AppC.IsFinished ())
        {
            return;
        }

        if (androidSDK != null)
        {
            androidSDK.Call("OpenRestore");
        }
    }

    public static void ConfirmRestored ()
    {
        if (!AppC.IsFinished())
        {
            return;
        }
        
        if (androidSDK != null)
        {
            androidSDK.Call("ConfirmRestored");
        }
    }

#elif UNITY_IOS
	[DllImport("__Internal")]
	private static extern void setupAppCWithMediaKey(string gameObjName, string mk, int opt_itemStore, int opt_push, int opt_datastore);

	[DllImport("__Internal")]
	private static extern string getInquiryKey();

	[DllImport("__Internal")]
	private static extern void openItemStore();

	[DllImport("__Internal")]
	private static extern string getGroupName(string groupID);

	[DllImport("__Internal")]
	private static extern int getItemCount(string key);

	[DllImport("__Internal")]
	private static extern void addItemCount(string key, int addCount);

	[DllImport("__Internal")]
	private static extern void setItemCount(string key, int setCount);

	[DllImport("__Internal")]
	private static extern void openRecoverGenerate();

	[DllImport("__Internal")]
	private static extern void openRecoverRestore();

	[DllImport("__Internal")]
	private static extern bool putDataStore(string key, string value);

	[DllImport("__Internal")]
	private static extern string getDataStore(string key);

	[DllImport("__Internal")]
    private static extern void confirmRestored();

	public void Init()
	{
		setupAppCWithMediaKey(this.name, iOSMediaKey, (ItemStore == null ? 0 : 1), (Push == null ? 0 : 1), (Datastore == null ? 0 : 1));
	}

	public static string GetInquiryKey()
	{
		try
		{
			return getInquiryKey();
		}
		catch
		{
			return null;
		}
	}

	// Androidでアプリ終了の機能を実装している場合はPluginDestroyを呼び出してください（Androidのみ）
	public static void PluginDestroy()
	{
		return;
	}

	public static void OpenRecoverGenerateView()
	{
		if (!AppC.IsFinished())
		{
			return;
		}

		openRecoverGenerate();
	}

	public static void OpenRecoverRestoreView()
	{
		if (!AppC.IsFinished())
		{
			return;
		}

		openRecoverRestore();
	}

    public static void ConfirmRestored()
	{
		if (!AppC.IsFinished())
		{
			return;
		}

        confirmRestored();
	}

#else
    /// <summary>
    /// appC Initialize.
    /// </summary>
    public void Init()
    {
        return;
    }

    /// <summary>
    /// Get the InquiryKey.
    /// </summary>
    public static string GetInquiryKey ()
    {
        return null;
    }

    /// <summary>
    /// Plugins the destroy. (Android only)
    /// </summary>
    public static void PluginDestroy () {
        return;
    }

    /// <summary>
    /// Opens the recover generate view.
    /// </summary>
    public static void OpenRecoverGenerateView ()
    {
        return;
    }

    /// <summary>
    /// Opens the recover restore view.
    /// </summary>
    public static void OpenRecoverRestoreView ()
    {
        return;
    }

    /// <summary>
    /// Checks the recovered.
    /// </summary>
    /// <returns><c>true</c>, if recovered was checked, <c>false</c> otherwise.</returns>
    public static bool ConfirmRestored ()
    {
        return false;
    }

#endif

    public void Awake()
    {
#if UNITY_EDITOR
		Debug.LogWarning("SDKの各機能はUnity Editorでは動作しませんので、実機にてご利用ください。");

		if (string.IsNullOrEmpty(androidMediaKey))
		{
			Debug.LogWarning("Androidのメディアキーが未入力です。Androidアプリをビルドする際には必須となりますのでご注意ください。");
		}

		if (string.IsNullOrEmpty(iOSMediaKey))
		{
			Debug.LogWarning("iOSのメディアキーが未入力です。iOSアプリをビルドする際には必須となりますのでご注意ください。");
		}

#elif UNITY_ANDROID
        if (string.IsNullOrEmpty (androidMediaKey))
        {
            Debug.LogWarning ("Androidのメディアキーが未入力です。");
        }
        
        // PluginName = PackageName + ClassName
        androidSDK = new AndroidJavaObject ("net.app_c.unity.appc");

#elif UNITY_IOS
        if (string.IsNullOrEmpty (iOSMediaKey))
        {
            Debug.LogWarning ("iOSのメディアキーが未入力です。");
        }

#endif

        // set API
        if (itemStoreAPI)
        {
            this.On(API.ITEMSTORE);
        }
        if (pushAPI)
        {
            this.On(API.PUSH);
        }
        if (datastoreAPI)
        {
            this.On(API.DATASTORE);
        }

        // appC Initialize
#if !UNITY_EDITOR
        this.Init();
#endif
    }

	/// <summary>
	/// ItemStore.
	/// </summary>
	public class _ItemStore
	{
#if UNITY_ANDROID
        public void OpenItemStoreView ()
        {
            if (!AppC.IsFinished ())
            {
                return;
            }
            
            if (androidSDK != null)
            {
                androidSDK.Call("OpenItemStore");
            }
        }

        public string GetGroupName (string id)
        {
            try
            {
                if (androidSDK != null)
                {
                    return androidSDK.Call<string>("GetGroupName", id);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public int GetItemCount (string id)
        {
            try
            {
                if (androidSDK != null)
                {
                    return androidSDK.Call<int>("GetItemCount", id);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public void AddItemCount (string id, int addCount)
        {
            if (androidSDK != null)
            {
                androidSDK.Call("AddItemCount", id, addCount);
            }
        }

        public void SetItemCount (string id, int setCount)
        {
            if (androidSDK != null)
            {
                androidSDK.Call("SetItemCount", id, setCount);
            }
        }
        
#elif UNITY_IOS
		public void OpenItemStoreView()
		{
			if (!AppC.IsFinished())
			{
				return;
			}

			openItemStore();
		}

		public string GetGroupName(string id)
		{
			try
			{
				return getGroupName(id);
			}
			catch
			{
				return null;
			}
		}

		public int GetItemCount(string id)
		{
			try
			{
				return getItemCount(id);
			}
			catch
			{
				return 0;
			}
		}

		public void AddItemCount(string id, int addCount)
		{
			addItemCount(id, addCount);
		}

		public void SetItemCount(string id, int setCount)
		{
			setItemCount(id, setCount);
		}

#else
        /// <summary>
        /// Opens the itemstore view.
        /// </summary>
        public void OpenItemStoreView () {}

        /// <summary>
        /// Gets the group name.
        /// </summary>
        /// <returns>Group name.</returns>
        /// <param name="id">Identifier.</param>
        public string GetGroupName (string id) { return null; }

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <returns>Item count.</returns>
        /// <param name="id">Identifier.</param>
        public int GetItemCount (string id) { return 0; }

        /// <summary>
        /// Adds the item count.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="addCount">Add count.</param>
        public void AddItemCount (string id, int addCount) {}

        /// <summary>
        /// Sets the item count.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="setCount">Set count.</param>
        public void SetItemCount (string id, int setCount) {}

#endif
	}

	/// <summary>
	/// Push.
	/// </summary>
	public class _Push
	{
	}

	/// <summary>
	/// Datastore.
	/// </summary>
	public class _Datastore
	{
#if UNITY_ANDROID
        public bool PutDataStore(string key, string value)
        {
            try
            {
                if (androidSDK != null)
                {
                    return androidSDK.Call<bool>("PutDataStore", key, value);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string GetDataStore(string key)
        {
            try
            {
                if (androidSDK != null)
                {
                    return androidSDK.Call<string>("GetDataStore", key);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
#elif UNITY_IOS
		public bool PutDataStore(string key, string value)
		{
			try
			{
				return putDataStore(key, value);
			}
			catch
			{
				return false;
			}
		}

		public string GetDataStore(string key)
		{
			try
			{
				return getDataStore(key);
			}
			catch
			{
				return null;
			}
		}
#else
        /// <summary>
        /// Puts the datastore.
        /// </summary>
        /// <returns><c>true</c>, if datastore was put, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public bool PutDataStore(string key, string value)
        {
            return false;
        }

        /// <summary>
        /// Gets the datastore.
        /// </summary>
        /// <returns>The datastore.</returns>
        /// <param name="key">Key.</param>
        public string GetDataStore(string key)
        {
            return null;
        }
#endif
	}
}