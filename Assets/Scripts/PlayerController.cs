using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// Class ref
	private Rigidbody rb;
	private PlayerInputController input;
	public Animator animator;
	public static PlayerController Instance;

	// Transform refs
	private Transform cameraTransform;

	[Header("PLAYER")]
	public float moveSpeed;
	public float jumpForce;
	public float rotationSpeed;
	public LayerMask groundMask;
	private Vector2 moveInput;
	private Vector3 moveDirection;

	[HideInInspector]
	public bool isAttacking = false;


	// ANIMATIONS
	private int runHash;
	private int jumpHash;
	private int attackHash;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		input = new PlayerInputController();
		input.Enable();

		input.Player.Jump.performed += OnJumpClick;
		input.Player.Attack.performed += OnAttackClick;

		rb = GetComponent<Rigidbody>();


		// ANIMATIONS HASHES
		runHash = Animator.StringToHash("Speed");
		jumpHash = Animator.StringToHash("Jump");
		attackHash = Animator.StringToHash("Attack");


		// Transform refs
		cameraTransform = Camera.main.transform;

		// Setting up variables
	}

	private void Update()
	{
		HandleMovement();
		Animations();
	}
	private void HandleMovement()
	{
		moveInput = input.Player.Movement.ReadValue<Vector2>();

		moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

		moveDirection = moveDirection.x * cameraTransform.transform.right.normalized + moveDirection.z * cameraTransform.transform.forward.normalized;
		moveDirection.y = 0;

		if (moveInput != Vector2.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		}


		// Make sure the player don't move when attacking
		if (!isAttacking)
		{
			rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
		}
	}
	private void Animations()
	{
		// Run
		animator.SetFloat(runHash, moveInput.normalized.magnitude);

		// Jump
		animator.SetBool(jumpHash, !IsGrounded());
	}

	#region Methods
	public bool IsGrounded()
	{
		// A simple check to see if the player is grounded. This can be adjusted based on the player's collider size and the ground detection method.
		return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
	}

	/*private IEnumerator AttackTimer()
	{
		yield return new WaitForSeconds()
	}*/


	
	#endregion
	#region OnButtonClick
	public void OnJumpClick(InputAction.CallbackContext callbackContext)
	{
		if (callbackContext.performed && IsGrounded())
		{
			// Add the jump velocity to the current velocity
			rb.velocity += new Vector3(0, jumpForce, 0);

			print("JUMPED");
		}
	}
	public void OnAttackClick(InputAction.CallbackContext callbackContext)
	{
		if (callbackContext.performed && !isAttacking)
		{
			// Attack
			animator.SetTrigger(attackHash);

			print("ATTACKED");

		}
	}
	#endregion
}
