using System;
using PathCreation;
using UnityEngine;

namespace Shadout.Controllers
{
	public abstract class ContenderMovementBase : MonoBehaviour
	{
		#region SerializedFields
		
		[Header("Run")]

		[SerializeField] protected float runSpeed = 5;
		[SerializeField] protected float runSideMoveSpeed = 5;

		[Header("Fly")]

		[SerializeField] protected float flySpeed;
		[SerializeField] protected float flySideMoveSpeed;

		[Header("Skate")]

		[SerializeField] private float skateSpeed;

		#endregion

		#region Variables
		
		protected ContenderBase contender;
		protected PathCreator pathCreator;
		protected Rigidbody rb;
		protected Transform followerObject;
		protected Transform referenceObject;
		protected float distanceTravelled;
		protected float currentSpeed;
		[HideInInspector] public float playerHeight;

		#endregion

		#region Props
		
		public float DistanceTravelled => distanceTravelled;
		public float FlySpeed => flySpeed;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			pathCreator = PathManager.Instance.PathCreator;
			var position = transform.position;

			referenceObject = new GameObject("referenceObject").transform;
			referenceObject.position = pathCreator.path.GetClosestPointOnPath(position);

			followerObject = new GameObject("followerObject").transform;
			followerObject.position = position - transform.forward * -.2f;

			distanceTravelled = 5;
			contender = GetComponent<ContenderBase>();
			rb = GetComponent<Rigidbody>();

			contender.ContenderStateChanged += OnContenderStateChanged;
		}

        protected virtual void Update()
		{
			distanceTravelled += currentSpeed * Time.deltaTime;
		}

		#endregion

		#region Methods
		
		protected void SetMovementSkate()
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

        private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
        {
            if (newState == ContenderState.End)
			{
				currentSpeed = 0;
			}
        }

		#endregion
	}
}
