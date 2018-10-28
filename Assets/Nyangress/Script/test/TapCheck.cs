using System;
using TouchScript.Gestures;
using UnityEngine;

public class TapChck : MonoBehaviour
{

    // オブジェクトが有効化されたときにイベントハンドラを登録する
    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += tappedHandler;
    }

    // オブジェクトが無効化されたときにイベントハンドラを削除する
    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
    }

    // タップイベントのイベントハンドラ
    private void tappedHandler(object sender, EventArgs e)
    {
        Debug.Log("is tapped");
    }
}