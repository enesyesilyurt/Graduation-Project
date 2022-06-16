using System;
using Shadout.Utilities;
using UnityEngine;

namespace Shadout.Models
{
	public class LevelManager : Singleton<LevelManager>
	{
		#region SerializedFields

		[SerializeField]
		private GameObject[] levels;

		#endregion

		#region Variables

		private int index;
		private GameObject level;
		private GameObject currentLevelPrefab;

		#endregion

		#region Events

		public event Action levelCompleted;

		#endregion

		#region Props

		public GameObject Level => level;

		#endregion

		#region Unity Methods

        #endregion

        #region Methods

		public void InitLevelManager()
		{
			for (int i = 0; i < levels.Length; i++)
			{
				levels[i].SetActive(false);
			}
			level = levels[0];
			currentLevelPrefab = level;
			currentLevelPrefab.SetActive(true);

			PathManager.Instance.InitPathManager();
		}

		public void GetNextLevel()
		{
			currentLevelPrefab.SetActive(false);
			level = levels[++index];
			currentLevelPrefab = level;
			currentLevelPrefab.SetActive(true);
			levelCompleted?.Invoke();

			GameManager.Instance.UpdateGameState(GameStates.Start);
		}

		public void RestartLevel()
		{
			levelCompleted?.Invoke();

			GameManager.Instance.UpdateGameState(GameStates.Start);
		}

        #endregion

        #region Callbacks

        #endregion
    }
}