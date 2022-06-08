using UnityEngine;

namespace Shadout.Controllers
{
	public class ContenderVehicleController : MonoBehaviour
	{
		#region SerializedFields
		
		[SerializeField] private GameObject wing;
		[SerializeField] private GameObject skateBoard;

		#endregion

		#region Variables
		
		private ContenderBase contender;

		#endregion

		#region Unity Methods
		
		private void Awake() 
		{
			contender = GetComponent<ContenderBase>();

			contender.ContenderStateChanged += OnContenderStateChanged;
		}

		#endregion

		#region Methods
		
		private void OpenSkate()
		{
			skateBoard.transform.localScale = Vector3.one * 0.01f;
			skateBoard.LeanScale(Vector3.one, .7f).setEaseOutCubic();
			skateBoard.SetActive(true);
		}
		
		private void OpenWing()
		{
			wing.SetActive(true);
			wing.transform.localScale = Vector3.one * 0.01f;
			wing.transform.LeanScale(Vector3.one, .2f).setEaseInOutSine();
		}
		
		private void CloseWing()
		{
			wing.transform.LeanScale(Vector3.one * 0.01f, .2f).setEaseInOutSine().setOnComplete(() => 
			{ 
				wing.SetActive(false); 
			});
		}
		
		private void CloseSkate()
		{
			skateBoard.SetActive(false);
		}

		#endregion

		#region Callbacks
		
		private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
		{
			switch (currentState)
			{
				case ContenderState.Skate:
					CloseSkate();
					break;
				case ContenderState.Fly:
					CloseWing();
					break;
			}

			switch (newState)
			{
				case ContenderState.Skate:
					OpenSkate();
					break;
				case ContenderState.Fly:
					OpenWing();
					break;
			}
		}

		#endregion
	}
}