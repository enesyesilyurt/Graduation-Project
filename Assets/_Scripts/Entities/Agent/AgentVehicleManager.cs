using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVehicleManager : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] private GameObject wing;
    [SerializeField] private GameObject skateBoard;

	#endregion

	#region Variables

	private Agent player;

	#endregion

	#region Unity Methods

	private void Awake() 
	{
		player = GetComponent<Agent>();

		player.PlayerStateChanged += OnPlayerStateChanged;
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

	private void OnPlayerStateChanged(PlayerStates currentState, PlayerStates newState)
    {
		switch (currentState)
        {
            case PlayerStates.Skate:
				CloseSkate();
                break;
            case PlayerStates.Fly:
				CloseWing();
                break;
        }

        switch (newState)
        {
            case PlayerStates.Skate:
				OpenSkate();
                break;
            case PlayerStates.Fly:
				OpenWing();
                break;
        }
    }

	#endregion
}
