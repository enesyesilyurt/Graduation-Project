using Shadout.Utilities;
using UnityEngine;

namespace Shadout.Models
{
	public class SaveSystem : Singleton<SaveSystem>
	{
		#region SerializedFields

		#endregion

		#region Variables

		#endregion

		#region Events

		#endregion

		#region Props

		#endregion

		#region Unity Methods

		#endregion

		#region Methods

		public void InitSaveSystem()
		{
			//save system....
			
			UIManager.Instance.InitUIManager();
			LevelManager.Instance.InitLevelManager();
		}

		#endregion

		#region Callbacks

		#endregion
	}
}