using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewCameraController : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = GetComponent<Camera>();
        if (camera != null && player != null)
        {
            camera.transform.position = player.transform.position;
        }
    }
}
