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
		protected ContenderAnimationController contenderAnimationController;
		protected ContenderVehicleController contenderVehicleController;

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

		#endregion

		#region Methods

		public virtual void InitContender()
		{
			contenderMovement = GetComponent<ContenderMovementBase>();
			contenderAnimationController = GetComponent<ContenderAnimationController>();
			contenderVehicleController = GetComponent<ContenderVehicleController>();

			contenderAnimationController.Init();
			contenderVehicleController.Init();
			contenderMovement.Init();

			GameManager.Instance.GameStateChanged += OnGameStateChanged;
		}
		
		public void UpdateContenderState(ContenderState newState)
		{
			ContenderStateChanged?.Invoke(currentContenderState, newState);
			currentContenderState = newState;
		}

		
    	public void OnWinGame(int count, Vector3 newPosition)
    	{
        	UpdateContenderState(ContenderState.End);

			transform.LeanMoveX(newPosition.x, 1).setEaseOutSine();
			transform.LeanMoveY(newPosition.y, 1).setEaseOutSine();
			transform.LeanMoveZ(newPosition.z, 1).setEaseOutBack();

			transform.LeanRotateAround(Vector3.up, 180, 1);
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
					UpdateContenderState(ContenderState.End);
					break;
			}
		}

		#endregion
	}
}