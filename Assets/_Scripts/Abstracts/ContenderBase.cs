using System;
using UnityEngine;

namespace Shadout.Controllers
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(ContenderAnimationController))]
	[RequireComponent(typeof(ContenderVehicleController))]
	public abstract class ContenderBase : MonoBehaviour
	{
		#region Variables

		protected ContenderMovementBase contenderMovement;
		protected ContenderState currentContenderState = ContenderState.End;

		#endregion

		#region Events
		
		public event Action<ContenderState, ContenderState> ContenderStateChanged;

		#endregion

		#region Props
		
		public float DistanceTravelled => contenderMovement.DistanceTravelled;
		public ContenderState CurrentContenderState => currentContenderState;

		#endregion

		#region Unity Methods

		protected virtual void Awake()
		{
			contenderMovement = GetComponent<ContenderMovementBase>();

			GameManager.Instance.GameStateChanged += OnGameStateChanged;
		}

		#endregion

		#region Methods
		
		public void UpdateContenderState(ContenderState newState)
		{
			ContenderStateChanged?.Invoke(currentContenderState, newState);
			currentContenderState = newState;
		}

		#endregion

		#region Callbacks
		
		private void OnGameStateChanged(GameStates newState)
		{
			switch (newState)
			{
				case GameStates.Start:
					UpdateContenderState(ContenderState.WaitStart);
					break;
				case GameStates.Game:
					UpdateContenderState(ContenderState.Run);
					break;
				case GameStates.End:
					break;
			}
		}

		#endregion
	}
}