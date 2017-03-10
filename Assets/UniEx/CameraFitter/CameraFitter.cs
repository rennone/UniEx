using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitter : MonoBehaviour
{
    void Sync()
    {
        
    }

    void Awake()
    {
        Sync();
    }

#if UNITY_EDITOR
    void Update()
    {
        Sync();
    }
#endif
}
