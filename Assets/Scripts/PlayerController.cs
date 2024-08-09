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

	[Header("PLAYER")]
	public float moveSpeed;
	public float jumpForce;
	public LayerMask groundMask;
	private Vector2 moveInput;
	private Vector3 moveDirection;

	// ANIMATIONS
	private int runHash;

	private void Start()
	{
		input = new PlayerInputController();
		input.Enable();

		input.Player.Jump.performed += OnJumpClick;


		rb = GetComponent<Rigidbody>();


		// ANIMATIONS HASHES
		runHash = Animator.StringToHash("Speed");
	}

	private void Update()
	{
		HandleMovement();
	}
	private void HandleMovement()
	{
		moveInput = input.Player.Movement.ReadValue<Vector2>();

		moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

		rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

		// Animation
		animator.SetFloat(runHash, moveInput.normalized.magnitude);
	}

	#region Methods

	public bool IsGrounded()
	{
		// A simple check to see if the player is grounded. This can be adjusted based on the player's collider size and the ground detection method.
		return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
	}
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
	#endregion
}
