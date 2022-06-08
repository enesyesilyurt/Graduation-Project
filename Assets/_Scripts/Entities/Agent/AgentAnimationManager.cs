using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimationManager : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] private Animator animator;

	#endregion

	#region Variables

    private Agent player;
    private string currentAnimName;

	#endregion

	#region Unity Methods

    private void Awake() 
    {
        player = GetComponent<Agent>();

		player.PlayerStateChanged += OnPlayerStateChanged;
    }

	#endregion

	#region Methods

	private void SetAnimations(string newAnimName)
    {
        if (currentAnimName != null)
        {
            animator.ResetTrigger(currentAnimName);
        }
        
        animator.SetTrigger(newAnimName);
        currentAnimName = newAnimName;
    }
	
	#endregion

	#region Callbacks

	private void OnPlayerStateChanged(PlayerStates currentState, PlayerStates newState)
    {
        switch (newState)
        {
            case PlayerStates.WaitStart:
				SetAnimations("Idle");
                break;
            case PlayerStates.Run:
				SetAnimations("Run");
                break;
            case PlayerStates.Skate:
				SetAnimations("Skate");
                break;
            case PlayerStates.Fly:
				SetAnimations("Fly");
                break;
            case PlayerStates.End:
				SetAnimations("Idle");
                break;
        }
    }

	#endregion
}
