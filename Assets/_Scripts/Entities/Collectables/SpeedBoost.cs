using Shadout.Controllers;

public class SpeedBoost : Collectable
{
    protected override void Collect(ContenderBase contender)
    {
        base.Collect(contender);
        contender.GetComponent<ContenderMovementBase>().BoostSpeed(2f);
    }
}
