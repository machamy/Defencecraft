﻿using UnityEngine;

/// <summary>
/// 화면 비율 고정.
/// TODO: 비율이 하드코딩 되어있음 ㅎㅎ;
/// </summary>
public class CameraResolution: MonoBehaviour
{
    public Vector2 resolution = new Vector2(20,9);
    
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)resolution.x / resolution.y); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }

        camera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}