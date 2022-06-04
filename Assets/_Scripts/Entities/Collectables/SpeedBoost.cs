public class SpeedBoost : Collectable
{
    protected override void Collect(PlayerManager player)
    {
        base.Collect(player);
        player.GetComponent<PlayerMovementManager>().BoostSpeed(2f);
    }
}
