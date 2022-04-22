using System;
using Helpers;
using UnityEngine;

public class InputController : MonoSingleton<InputController>
{
    public float SideInput { get; private set; }

    public event Action Clicked;

    private bool isFirstTouch = false;

    private void Update() 
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.touchCount <= 0)
        {
            SideInput = 0;
            isFirstTouch = true;
            return;
        }
            
        Touch touch = Input.GetTouch(0);
        SideInput = touch.deltaPosition.x;

        if(!isFirstTouch) return;
        isFirstTouch = false;
        Clicked?.Invoke();
    }
}
