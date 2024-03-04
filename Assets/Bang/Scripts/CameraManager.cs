using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float horizontalResolution = 1920f;
    public float verticalResolution = 1080f;
    public bool resize = true;
    private float ortoDestine;
    private float ortoInit;
    private float lastScreenWidth;
    private float lastScreenHeight;

    private void Awake()
    {
        this.ortoInit = Camera.main.orthographicSize;
        Camera.main.orthographicSize = 5.4f;
    }
    private void Update()
    {
        if (Time.frameCount % 24 != 0 || (double) this.lastScreenWidth == (double) Screen.width && (double) this.lastScreenHeight == (double) Screen.height)
            return;
        this.lastScreenWidth = (float)Screen.width;
        this.lastScreenHeight = (float)Screen.height;
        this.resize = true;
        if (!this.resize)
            return;
        float num1 = (float)Screen.width / (float)Screen.height / 1.77777779f;
        //Camera component = this.GetComponent<Camera>;
        if((double) num1 < 1.0)
        {
        }
    }

}
