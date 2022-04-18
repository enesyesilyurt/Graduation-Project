using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Collectable
{
    private void Start() 
    {
        
    }
    protected override void Collect()
    {
        base.Collect();
        Player.Instance.BoostSpeed(1.5f);
    }
}
