using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyNormal : MonoBehaviour
{
    public UnityEngine.Vector3 mNormal;

    public Vector3 GetResult()
    {
        return new Vector3()
        {
            x = mNormal.x,
            y = mNormal.y,
            z = mNormal.z
        };
    }
}
