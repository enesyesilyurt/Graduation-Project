using System;
using PathCreation;
using UnityEngine;

namespace Shadout.Controllers
{
    public class AgentMovement : MonoBehaviour
    {
        #region SerializedFields

        [Header("Run")]

        [SerializeField] private float runSpeed = 5;
        [SerializeField] private float runSideMoveSpeed = 5;

        [Header("Fly")]

        [SerializeField] private float flySpeed;
        [SerializeField] private float flySideMoveSpeed;

        [Header("Skate")]

        [SerializeField] private float skateSpeed;

        #endregion

		public float FlySpeed => flySpeed;
		public float RunSideMoveSpeed => runSideMoveSpeed;
		public Transform FollowerObject => followerObject;
		public Transform ReferanceObject => referanceObject;

        #region Variables

		public float SideInput = 1f;

        private Agent player;
        private PathCreator pathCreator;
        private Rigidbody rb;
        private Transform followerObject;
        private Transform referanceObject;
        private float distanceTravelled;
        private float currentSpeed;
        public float playerHeight;

        #endregion

        #region Props

        public float DistanceTravelled => distanceTravelled;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            pathCreator = PathManager.Instance.PathCreator;
            var position = transform.position;

            referanceObject = new GameObject("referanceObject").transform;
            referanceObject.position = pathCreator.path.GetClosestPointOnPath(position);

            followerObject = new GameObject("followerObject").transform;
            followerObject.position = position - transform.forward * -.2f;

            distanceTravelled = 5;
            player = GetComponent<Agent>();
            rb = GetComponent<Rigidbody>();

            player.PlayerStateChanged += OnPlayerStateChanged;
        }

        private void Update()
        {
            distanceTravelled += currentSpeed * Time.deltaTime;
        }

        #endregion

        #region Methods

        private void SetMovementSkate()
        {
            currentSpeed = skateSpeed;
            playerHeight = .25f;
        }

        public void BoostSpeed(float boost)
        {
            var tempSpeed = currentSpeed;
            currentSpeed *= boost;
            LeanTween.delayedCall(2, () =>
            {
                if (Math.Abs(currentSpeed * boost - tempSpeed) < 0.01f)
                    currentSpeed = tempSpeed;
            });
        }

        #endregion

        #region Callbacks

        private void OnPlayerStateChanged(PlayerStates currentState, PlayerStates newState)
        {
            switch (newState)
            {
                case PlayerStates.WaitStart:
					distanceTravelled = PathManager.Instance.PathCreator.path.GetClosestDistanceAlongPath(transform.position);
                    break;
                case PlayerStates.Run:
                    distanceTravelled = PathManager.Instance.PathCreator.path.GetClosestDistanceAlongPath(transform.position);
					currentSpeed = runSpeed;
                    break;
                case PlayerStates.Skate:
                    SetMovementSkate();
                    break;
                case PlayerStates.Fly:
                    break;
                case PlayerStates.End:
                    break;
            }
        }

        #endregion
    }
}