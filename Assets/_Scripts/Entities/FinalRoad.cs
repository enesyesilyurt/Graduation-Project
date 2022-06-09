using System;
using UnityEngine;

namespace Shadout.Controllers
{
	public class FinalRoad : MonoBehaviour
	{
		#region SerializedFields

		[SerializeField]
		private Transform firstPlace;

		[SerializeField]
		private Transform secondPlace;
		
		[SerializeField]
		private Transform thirdPlace;

		#endregion

		#region Variables

		private int index = 0;

		#endregion

		#region Events

		#endregion

		#region Props

		#endregion

		#region Unity Methods

		private void Awake() 
		{
			GameManager.Instance.GameStateChanged += OnGameStateChanged;
		}

		private void OnTriggerEnter(Collider other) 
		{
			var contender = other.GetComponent<ContenderBase>();
			if (contender != null)
			{
				switch (index)
				{
					case 0:
						contender.OnWinGame(index, firstPlace.position);
						break;
					case 1:
						contender.OnWinGame(index, secondPlace.position);
						break;
					case 2:
						contender.OnWinGame(index, thirdPlace.position);
						GameManager.Instance.UpdateGameState(GameStates.End);
						break;
				}
				index++;
			}
		}

        #endregion

        #region Methods

        #endregion

        #region Callbacks

        private void OnGameStateChanged(GameStates newState)
        {
            switch (newState)
			{
				case GameStates.Start:
					index = 0;
					break;
				case GameStates.Game:
					break;
				case GameStates.End:
					break;
			}
        }

        #endregion
    }
}