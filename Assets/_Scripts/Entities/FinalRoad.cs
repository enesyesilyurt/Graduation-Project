using System;
using Shadout.Models;
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

        [SerializeField]
        private Transform particlesParent;

        [SerializeField]
        private Transform radialBackGround;

        #endregion

        #region Variables

        private Vector3 radialBackGroundScale;
        private int index = 0;

        #endregion

        #region Events

        #endregion

        #region Props

        #endregion

        #region Unity Methods

        private void Awake() {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
            LevelManager.Instance.levelCompleted += ()=> index = 0;
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
                        if (GameManager.Instance.CurrentState != GameStates.End)
                        {
                            GameManager.Instance.UpdateGameState(GameStates.End);
                        }
                        break;
                }
                index++;
            }
        }

        public void InitFinalRoad()
        {
            radialBackGroundScale = radialBackGround.localScale;
            InitAnimatedObject();
        }

        public void InitAnimatedObject()
        {
            index = 0;
            particlesParent.gameObject.SetActive(false);
            transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(-.001f);
            transform.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(-1f));
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y -180 , transform.eulerAngles.z);
        }

        public void OpenAnimatedObjects()
        {
            particlesParent.gameObject.SetActive(true);
            radialBackGround.localScale = radialBackGroundScale;
        }

        #endregion

        #region Methods

        #endregion

        private void OnGameStateChanged(GameStates newState)
        {
            switch (newState)
            {
                case GameStates.End:
                    OpenAnimatedObjects();
                    break;
            }
        }

        #region Callbacks

        #endregion
    }
}