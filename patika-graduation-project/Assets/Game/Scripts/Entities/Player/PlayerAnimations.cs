using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	#region SerializedFields
	
    [SerializeField]
    private Animator animator;

	#endregion

	#region Variables

	private Player player;
    private string currentAnimName;

	#endregion

	#region Props

	#endregion

	#region Unity Methods

	#endregion

	#region Methods

	private void SetAnimations(string newAnimName)
    {
        animator.ResetTrigger(currentAnimName);
        animator.SetTrigger(newAnimName);
        currentAnimName = newAnimName;
    }

	private void IdleAnim()
    {
        SetAnimations("Idle");
    }

	private void FlyAnim()
    {
        SetAnimations("Fly");
    }

	private void SkateAnim()
    {
        SetAnimations("Skate");
    }

	private void RunAnim()
    {
        SetAnimations("Run");
    }
	
	

	#endregion

	#region Callbacks

	#endregion
}