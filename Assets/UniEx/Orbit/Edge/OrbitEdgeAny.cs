using UnityEngine;
using System.Collections;
using System;

namespace UniEx
{
    // 任意の関数を入れる
    public class OrbitEdgeAny : OrbitEdge
    {
        public override Vector2 Start
        {
            get { return startFunc_(); }
        }

        public override Vector2 Dest
        {
            get { return destFunc_(); }
        }

        Func<Vector2> destFunc_;
        Func<Vector2> startFunc_;

        public OrbitEdgeAny(Func<Vector2> startFunc, Func<Vector2> destFunc)
        {
            startFunc_ = startFunc;
            destFunc_ = destFunc;
        }
    }


    public class OrbitEdgeAnyDest : OrbitEdgeAny
    {
        public OrbitEdgeAnyDest(Vector2 start, Func<Vector2> destFunc)
            : base(() => start, destFunc)
        {
        }
    }
}