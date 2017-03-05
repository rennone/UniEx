using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleSystemEx
{
	/// <summary> 拡張機能: emission.rateOverTimeのconstantMin,Max をemitNumに差し替え </summary>
	public static void SetConstantEmissionNum(this ParticleSystem self, int emitNum)
	{
		if(self != null)
		{
			var e = self.emission;
			var r = e.rateOverTime;
			r.constantMin = emitNum;
			r.constantMax = emitNum;
			e.rateOverTime = r;
		}
	}

    public static float GetTime(this ParticleSystem self)
    {
        // パーティクルシステムがアタッチされている
        if (self == null)
            return 0f;

        var ret  = self.time;

        // 継続時間 + 開始ディレイ + 各パーティクルのライフタイム
        // TODO : 他にもあるか確認 & ライフタイムが曲線設定の場合おかしなことになるかもしれない
        if (!self.main.loop)
            ret = self.main.duration + self.main.startDelay.constant + self.main.startLifetime.constant;

        return ret;
    }
}
