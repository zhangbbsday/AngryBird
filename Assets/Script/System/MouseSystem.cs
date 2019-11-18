using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSystem : BaseSystem
{
    private Texture2D normalCursor;
    private Texture2D clickCursor;
    private Texture2D pressCursor;
    public MouseSystem(Texture2D[] cursors)
    {
        normalCursor = cursors[0];
        clickCursor = cursors[1];
        pressCursor = cursors[2];
    }

    public override void Release()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        if (Input.GetMouseButton(0))
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    protected override void Initialize()
    {
        throw new System.NotImplementedException();
    }
}
