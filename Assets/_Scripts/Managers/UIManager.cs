using System;
using NaughtyAttributes;
using Shadout.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Shadout.Models
{
	public class UIManager : Singleton<UIManager>
	{
		#region SerializedFields

		[Foldout("End Panel")] [SerializeField] private GameObject endPanel;
		[Foldout("End Panel")] [SerializeField] private Button nextLevelButton;
		[Foldout("End Panel")] [SerializeField] private Button restartButton;

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

		public void InitUIManager()
		{
			GameManager.Instance.GameStateChanged += OnGameStateChanged;
			nextLevelButton.onClick.AddListener(()=> LevelManager.Instance.GetNextLevel());
		}

        #endregion

        #region Callbacks

        private void OnGameStateChanged(GameStates newState)
        {
            switch (newState)
			{
				case GameStates.Start:
					endPanel.SetActive(false);
					break;
				case GameStates.End:
					endPanel.SetActive(true);
					Debug.Log("game end");
					break;
			}
        }

        #endregion
    }
}