using System;
using Shadout.Utilities;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

namespace Shadout.Models
{
	[Serializable]
	public class Level
	{
		public GameObject LevelPrefab;
		public PathCreator PathCreator;
		public RoadMeshCreator RoadMeshCreator;
		public int CollectablePercent;
	}

	public class LevelManager : Singleton<LevelManager>
	{
		#region SerializedFields

		[SerializeField]
		private Level[] levels;

		#endregion

		#region Variables

		private int index;
		private Level level;
		private GameObject currentLevelPrefab;

		#endregion

		#region Events

		public event Action levelCompleted;

		#endregion

		#region Props

		public Level Level => level;

		#endregion

		#region Unity Methods

        #endregion

        #region Methods

		public void InitLevelManager()
		{
			level = levels[0];
			currentLevelPrefab = Instantiate(levels[0].LevelPrefab,Vector3.zero,Quaternion.identity);

			PathManager.Instance.InitPathManager();
		}

		public void GetNextLevel()
		{
			Destroy(currentLevelPrefab);
			currentLevelPrefab = Instantiate(levels[++index].LevelPrefab,Vector3.zero,Quaternion.identity);
			levelCompleted?.Invoke();
		}

        #endregion

        #region Callbacks

        #endregion
    }
}