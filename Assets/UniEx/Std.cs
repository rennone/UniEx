using UnityEngine;
using System.Collections;

public static class Std
{
	public static void Swap<T>(ref T a, ref T b)
	{
		T t = a;
		a = b;
		b = t;
	}
}
