using UnityEngine;

namespace Shadout.Controllers
{
	public class ContenderAnimationController : MonoBehaviour
	{
		#region SerializedFields
		
		[SerializeField] private Animator animator;

		#endregion

		#region Variables
		
		private ContenderBase contender;
		private string currentAnimName;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			contender = GetComponent<ContenderBase>();

			contender.ContenderStateChanged += OnContenderStateChanged;
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
		
		private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
		{
			switch (newState)
			{
				case ContenderState.WaitStart:
					SetAnimations("Idle");
					break;
				case ContenderState.Run:
					SetAnimations("Run");
					break;
				case ContenderState.Skate:
					SetAnimations("Skate");
					break;
				case ContenderState.Fly:
					SetAnimations("Fly");
					break;
				case ContenderState.End:
					SetAnimations("Idle");
					break;
			}
		}

		#endregion
	}
}