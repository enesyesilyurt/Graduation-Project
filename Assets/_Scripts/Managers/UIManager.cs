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

		public bool IsWinGame = false;

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
			restartButton.onClick.AddListener(()=> LevelManager.Instance.RestartLevel());
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
					if (IsWinGame)
					{
						nextLevelButton.gameObject.SetActive(true);
						restartButton.gameObject.SetActive(false);
					}
					else
					{
						nextLevelButton.gameObject.SetActive(false);
						restartButton.gameObject.SetActive(true);
					}
					IsWinGame = false;
					break;
			}
        }

        #endregion
    }
}