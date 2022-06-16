using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAwake : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
