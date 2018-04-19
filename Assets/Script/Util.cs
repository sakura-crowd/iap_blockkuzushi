using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユーティリティ関数。 
/// </summary>
public class Util : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// AudioClip から AudioSource を作成して再生します。作成した AudioSource は再生時間の経過後に破棄されます。 
    /// 参照 Play sound on button click before loading level. - Unity Answers : https://answers.unity.com/questions/26684/play-sound-on-button-click-before-loading-level.html
    /// </summary>
    /// <param name="clip">再生する音源</param>
    /// <param name="position">再生するワールド座標</param>
    /// <param name="volume">再生する音量</param>
    /// <returns></returns>
    public static AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
    {
        if (clip == null)
        {
            return null;
        }

        GameObject go = new GameObject("One shot audio");
        DontDestroyOnLoad(go);
        go.transform.position = position;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.Play(); 
        Destroy(go, clip.length);
        return source;
    }

    /// <summary>
    /// URL の画面を開きます。 
    /// </summary>
    /// <param name="url"></param>
    public static void OpenURL(string url)
    {
        Application.OpenURL(url);
        return;
    }

}
