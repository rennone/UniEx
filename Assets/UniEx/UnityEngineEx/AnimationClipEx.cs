using UnityEngine;
using System.Collections;
using System.Linq;

namespace UniEx
{
    public static class AnimationClipEx
    {
        public static float GetCallbackTime(this AnimationClip clip, string callbackName)
        {
            var callback = clip.events.FirstOrDefault(e => e.functionName == callbackName);

            if (callback == null)
                return -1;

            return callback.time;
        }

        public static float GetCallbackNormalizedTime(this AnimationClip clip, string callbackName)
        {
            if (clip.length <= 0)
                return -1;

            var ret = GetCallbackTime(clip, callbackName);

            if (ret < 0)
                return ret;

            if (clip.length <= 0)
                return 0;

            return ret / clip.length;
        }
    }
}
