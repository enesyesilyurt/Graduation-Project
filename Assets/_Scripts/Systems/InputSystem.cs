using System;
using Shadout.Utilities;
using UnityEngine;

public class InputSystem : Singleton<InputSystem>
{
    public float SideInput { get; private set; }
    
    public event Action<Touch> Clicked;

    private void Update() 
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.touchCount <= 0)
        {
            SideInput = 0;
            return;
        }
            
        Touch touch = Input.GetTouch(0);
        SideInput = touch.deltaPosition.x;

        Clicked?.Invoke(touch);
    }
}
