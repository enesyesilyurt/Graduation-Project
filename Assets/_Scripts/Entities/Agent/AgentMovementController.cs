using System;
using PathCreation;
using UnityEngine;

namespace Shadout.Controllers
{
    public class AgentMovementController : ContenderMovementBase
    {
        #region Serializefields

        [SerializeField]
        private float sidePosition;

        #endregion

        #region Variables

		public float SideInput = 1f;

        #endregion

        #region Props
        
        public float RunSideMoveSpeed => runSideMoveSpeed;
        public float SidePosition => sidePosition;
        public Transform FollowerObject => followerObject;
        public Transform ReferenceObject => referenceObject;

        #endregion

        #region Unity Methods

        #endregion

        #region Methods

        public override void Init()
        {
            base.Init();
            contender.ContenderStateChanged += OnContenderStateChanged;
        }

        private void SetRotate(ContenderState newState)
        {
            transform.eulerAngles = Vector3.zero;
            transform.GetChild(0).transform.eulerAngles = Vector3.zero;

			if (newState == ContenderState.End)
			{
				transform.GetChild(0).transform.eulerAngles = Vector3.zero + Vector3.up * -90;
				transform.GetChild(0).transform.localPosition = Vector3.zero;
			}
        }

        #endregion

        #region Callbacks

        private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
        {
            switch (newState)
            {
                case ContenderState.WaitStart:
                    SetRotate(newState);
                    distanceTravelled = 5;

                    referenceObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled);
        		    referenceObject.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled + 1));

				    followerObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(DistanceTravelled - .2f) + Vector3.up * playerHeight + sidePosition * referenceObject.right;

                    transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(5) + sidePosition * referenceObject.right + playerHeight * transform.up;
        		    transform.rotation = referenceObject.rotation;
                    break;
                case ContenderState.Run:
                    SetRotate(newState);
                    distanceTravelled = PathManager.Instance.PathCreator.path.GetClosestDistanceAlongPath(transform.position);
					currentSpeed = runSpeed;
                    break;
                case ContenderState.Skate:
                    SetRotate(newState);
                    SetMovementSkate();
                    break;
                case ContenderState.Fly:
                    break;
                case ContenderState.End:
                    SetRotate(newState);
                    break;
            }
        }

        #endregion
    }
}