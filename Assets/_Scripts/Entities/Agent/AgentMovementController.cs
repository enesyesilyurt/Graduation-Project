using System;
using PathCreation;
using UnityEngine;

namespace Shadout.Controllers
{
    public class AgentMovementController : ContenderMovementBase
    {
        #region Variables

		public float SideInput = 1f;

        #endregion

        #region Props
        
        public float RunSideMoveSpeed => runSideMoveSpeed;
        public Transform FollowerObject => followerObject;
        public Transform ReferenceObject => referenceObject;

        #endregion

        #region Unity Methods

        private void Start()
        {
            contender.ContenderStateChanged += OnContenderStateChanged;
        }

        #endregion

        #region Callbacks

        private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
        {
            switch (newState)
            {
                case ContenderState.WaitStart:
					distanceTravelled = PathManager.Instance.PathCreator.path.GetClosestDistanceAlongPath(transform.position);
                    break;
                case ContenderState.Run:
                    distanceTravelled = PathManager.Instance.PathCreator.path.GetClosestDistanceAlongPath(transform.position);
					currentSpeed = runSpeed;
                    break;
                case ContenderState.Skate:
                    SetMovementSkate();
                    break;
                case ContenderState.Fly:
                    break;
                case ContenderState.End:
                    break;
            }
        }

        #endregion
    }
}