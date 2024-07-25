using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lessson2_Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            Debug.Log("ILRuntime ILRuntime...");
        });
    }
}
