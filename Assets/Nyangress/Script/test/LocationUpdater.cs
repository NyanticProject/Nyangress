using System.Collections;
using UnityEngine;

/// <summary>経緯度取得クラス</summary>
public class LocationUpdater : MonoBehaviour
{
    /// <summary>経緯度取得間隔（秒）</summary>
    private const float IntervalSeconds = 0.5f;

    /// <summary>ロケーションサービスのステータス</summary>
    private LocationServiceStatus locationServiceStatus;

    /// <summary>経度</summary>
    public float Longitude { get; private set; }

    /// <summary>経度</summary>
    public float Latitude { get; private set; }

    /// <summary>緯度経度情報が取得可能か</summary>
    /// <returns>可能ならtrue、不可能ならfalse</returns>
    public bool CanGetLonLat()
    {
        return Input.location.isEnabledByUser;
    }

    /// <summary>経緯度取得処理</summary>
    /// <returns>一定期間毎に非同期実行するための戻り値</returns>
    private IEnumerator Start()
    {
        while (true)
        {
            locationServiceStatus = Input.location.status;
            if (Input.location.isEnabledByUser)
            {
                switch (locationServiceStatus)
                {
                    case LocationServiceStatus.Stopped:
                        Input.location.Start();
                        break;
                    case LocationServiceStatus.Running:
                        Longitude = Input.location.lastData.longitude;
                        Latitude = Input.location.lastData.latitude;
                        break;
                    default:
                        break;
                }
            }

            yield return new WaitForSeconds(IntervalSeconds);
        }
    }
}