using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region SerializedFields

	[Header("Run")]

    [SerializeField] 
	private float runSpeed = 5;

    [SerializeField] 
	private float runSideMoveSpeed = 5;

	[Header("Fly")]

    [SerializeField] 
	private float flySpeed;

    [SerializeField] 
	private float flySideMoveSpeed;

    [Header("Skate")]

    [SerializeField] 
	private float skateSpeed;

	#endregion

	#region Variables

	private Player player;
	
    private float currentSpeed;
	
    private Transform followerObject;

    private Transform referanceObject;
	
    private float playerHeight;

    private float sideMove;
	
    private Rigidbody rb;

    private RoadMeshCreator roadMesh;
	
    private PathCreator pathCreator;

	#endregion

	#region Props

	#endregion

	#region Unity Methods

	#endregion

	#region Methods

	private void Awake() 
	{
		player = GetComponent<Player>();
		rb = GetComponent<Rigidbody>();
        roadMesh = PathController.Instance.RoadMeshCreator;
        pathCreator = PathController.Instance.PathCreator;
	}

	private void InitMovement()
    {
        referanceObject = Instantiate(new GameObject(), pathCreator.path.GetClosestPointOnPath(transform.position), Quaternion.identity).transform;
        followerObject = Instantiate(new GameObject(), transform.position, Quaternion.identity).transform;
    }

	private void FlyMovement()
    {
        float sideInput = InputController.Instance.SideInput;

        transform.Rotate(Vector3.up * sideInput * Time.deltaTime * flySideMoveSpeed);
        transform.eulerAngles = new Vector3(12, transform.eulerAngles.y, 0);

        rb.velocity = transform.forward * flySpeed * Time.deltaTime;
    }

	private void RunMovement()
    {
        float sideInput = InputController.Instance.SideInput;
        sideMove += sideInput * Time.deltaTime;

        referanceObject.position = pathCreator.path.GetPointAtDistance(player.DistanceTravelled);
        referanceObject.LookAt(pathCreator.path.GetPointAtDistance(player.DistanceTravelled + 1));

        transform.position = Vector3.Lerp
        (
            transform.position,
            pathCreator.path.GetPointAtDistance(player.DistanceTravelled) + referanceObject.right * sideMove * runSideMoveSpeed + Vector3.up * playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, transform.position, .2f);
        followerObject.LookAt(transform);
        transform.rotation = followerObject.rotation;
    }

	public void SetMovementRun()
    {
        player.DistanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        playerHeight = 0;
        currentSpeed =  runSpeed;

        rb.velocity = Vector3.zero;

        followerObject.position = pathCreator.path.GetPointAtDistance(player.DistanceTravelled - .2f);

        transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        transform.position = pathCreator.path.GetPointAtDistance(player.DistanceTravelled);
        transform.LookAt(pathCreator.path.GetPointAtDistance(player.DistanceTravelled + 1));

        sideMove = 0;
    }
	
	public void SetMovementSkate()
    {
        
        currentSpeed = skateSpeed;

        playerHeight = .25f;
    }

	public void SetMovementFly()
    {
        transform.LeanRotateX(12, .2f).setEaseInCubic();
        transform.LeanMoveY(transform.position.y + 1, .2f).setEaseInCubic();
    }

	#endregion

	#region Callbacks

	#endregion
}