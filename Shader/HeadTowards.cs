using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HeadTowards : MonoBehaviour
{
    public Material _faceMaterial;
    private void SetHeadDirection()
    {
        if (_faceMaterial)
        {
            _faceMaterial.SetVector("HeadForward", transform.forward);
            _faceMaterial.SetVector("HeadRight", transform.right);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetHeadDirection();
    }
}
