using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D[] cursorTexture;
    public int frameCount;
    public float frameRate;

    private int currentFrame;
    private float frameTimer;
    
    private void Start()
    {
        Cursor.SetCursor(cursorTexture[0], new Vector2(8, 8), CursorMode.Auto);
    }

    private void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTexture[currentFrame], new Vector2(8, 8), CursorMode.Auto);
            Debug.Log(currentFrame);
        }
    }
}
