#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
#define MOBILE
#endif

using UnityEngine;

namespace UniEx
{
    public class Geometry : MonoBehaviour
    {
        /// <summary> 大塚 </summary>
        public static readonly Location OtsukaLocation = new Location(35.731723, 139.728184);

        /// <summary> 新宿 </summary>
        public static readonly Location Shinjuku = new Location(35.698109, 139.708588);

        /// <summary> 品川 </summary>
        public static readonly Location Shinagawa = new Location(35.6276017, 139.7409166);

        // 最東端(南鳥島)
        public static readonly Location Easternmost = new Location(24.287248, 153.980808);

        // 最南端(沖ノ鳥島)
        public static readonly Location Southernmost = new Location(20.422226, 136.073868);

        // 最西端(与那国島)
        public static readonly Location Westernmost = new Location(24.464531, 123.008630);

        // 最北端(択捉島はGooglemapで出てこないので, 国後島にしてある)
        public static readonly Location Northernmost = new Location(44.403692, 146.481912);

        // 日本の領土内に入っているかどうか
        public static bool IsValidLocation(Location location)
        {
            return Westernmost.longitude_ < location.longitude_ && location.longitude_ < Easternmost.longitude_ &&
                   Southernmost.latitude_ < location.latitude_ && location.latitude_ < Northernmost.latitude_;
        }


#if TL_DEBUG
/// <summary> 地図上でのデバッグ移動用 </summary>
	static Vector2 DebugOffsetMeter { get; set; }
	
	/// <summary> クエストのテストで開始位置をずらすためのオフセット位置 </summary>
	static Location DebugOffsetLocation { get; set; }

	/// <summary> 開始緯度経度設定 </summary>
	public static void SetStartLocation(Location location)
	{
		Location lastLocation;
		if (LastLocationImpl(out lastLocation) == LocationServiceStatus.Running)
		{
			DebugOffsetLocation = location - lastLocation;
			DebugOffsetMeter = Vector2.zero;
		}
	}

	/// <summary> 現在地から強制移動[m] </summary>
	public static void AddDebugOffsetMeter(Vector2 offset)
	{
		DebugOffsetMeter += offset;
	}

	/// <summary> デバッグオフセットの適用 </summary>
	public static Location AdaptDebugOffset(Location location)
	{
		return (location + DebugOffsetLocation).MovedMeterLocation(DebugOffsetMeter);
	}

#else
        public static void SetStartLocation(Location location)
        {
        }

        public static void AddDebugOffsetMeter(Vector2 vector)
        {
        }

        public static Location AdaptDebugOffset(Location location)
        {
            return location;
        }
#endif

        /// <summary> 位置情報取得要求リクエスト数 </summary>
        static int RequestNum { get; set; }

        /// <summary> ユーザーが位置情報を許可しているか </summary>
        public static bool IsEnableByUser
        {
            get { return Input.location.isEnabledByUser; }
        }

        /// <summary> 加工されていない現在位置(メニューなどで使う)。クエスト中(アクションポイントや範囲外雲などはMapField.GetLastLocationを使う事) </summary>
        public static LocationServiceStatus LastRawLocation(out Location ret)
        {
            return LastLocationImpl(out ret);
        }

        // 実機用 ----------------------------------
        // 位置情報を取得可能かどうか
        public static bool LocationAvailable
        {
            get
            {
#if MOBILE
// Startが開始されていなければStartを実行
			if (Status == LocationServiceStatus.Stopped && RequestNum > 0)
				LocationStart();

			// Runningモードに移るまで待つ
			return Status == LocationServiceStatus.Running;
#else
                return true;
#endif
            }
        }

        /// <summary> GPS取得可能状態の取得 </summary>
        public static LocationServiceStatus Status
        {
            get
            {
#if MOBILE
			return Input.location.status;
#else
                return LocationServiceStatus.Running;
#endif
            }
        }

        // 現在の位置情報
        static LocationServiceStatus LastLocationImpl(out Location res)
        {
#if MOBILE
// 位置実行可能でなければ警告を出す
		if (LocationAvailable == false)
		{
			DebugLogger.LogWarning("Location is not avaiable status = " + Input.location.status, DebugLogger.Tag.Map);
			res = Location.null_;
			return Status;
		}

		res = AdaptDebugOffset(new Location(Input.location.lastData.latitude, Input.location.lastData.longitude));
		return Status;
#else
            res = AdaptDebugOffset(OtsukaLocation);
            return Status;
#endif
        }

        /// <summary> 位置情報取得開始 </summary>
        static void RequestStart()
        {
            RequestNum += 1;

            // 精度は5m
            // 更新間隔は5m移動したら
            if (RequestNum > 0)
            {
                LocationStart();
            }
        }

        /// <summary> 位置情報取得終了 </summary>
        static void RequestStop()
        {
            RequestNum -= 1;

            if (RequestNum <= 0)
            {
                LocationStop();
            }
        }

        static void LocationStart()
        {
            // TODO
            //if (Status != LocationServiceStatus.Stopped)
            //	return;
            
            Input.compass.enabled = true;
            Input.location.Start(10, 10);
        }

        static void LocationStop()
        {
            Input.compass.enabled = false;
            Input.location.Stop();
        }

        /// <summary> 生成時に位置情報取得を開始する </summary>
        protected virtual void Awake()
        {
            RequestStart();
        }

        /// <summary> 破壊時に位置情報取得を終了する </summary>
        void OnDestroy()
        {
            RequestStop();
        }

    }
}