using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroys this object if the current platform is WebGL.
public class WebGLDestroy : MonoBehaviour {

#if UNITY_WEBGL
    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject);	
	}
#endif

}
