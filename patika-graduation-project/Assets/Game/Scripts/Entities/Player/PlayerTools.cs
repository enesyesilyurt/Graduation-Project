using UnityEngine;

public class PlayerTools : MonoBehaviour
{
	#region SerializedFields

	[Header("Objects")]

    [SerializeField]
    private GameObject wing;

    [SerializeField]
    private GameObject skateBoard;

	#endregion

	#region Variables

	#endregion

	#region Props

	#endregion

	#region Unity Methods

	#endregion

	#region Methods

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

	#endregion

	#region Callbacks

	#endregion
}