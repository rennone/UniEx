using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UniEx
{

    public static class UInt32Ex
    {
        public static uint LeftShift(this uint self, int amount, uint fill)
        {
            return ((self << amount) | (fill >> amount));
        }

        public static uint RightShift(this uint self, int amount, uint fill)
        {
            return (self >> amount) | fill << amount;
        }
    }
}

