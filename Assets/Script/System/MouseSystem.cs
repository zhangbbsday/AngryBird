using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSystem
{
    private Texture2D normalCursor;
    private Texture2D clickCursor;
    private Texture2D pressCursor;

    public MouseSystem(Texture2D normal, Texture2D click, Texture2D press)
    {
        normalCursor = normal;
        clickCursor = click;
        pressCursor = press;
    }

    public void UpdateCursor()
    {
        if (Input.GetMouseButton(0))
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }
}
