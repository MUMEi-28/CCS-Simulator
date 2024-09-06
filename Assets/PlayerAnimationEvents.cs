using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
	public void OnAttackAnimationEnd()
	{
		PlayerController.Instance.isAttacking = false;
	}
	public void OnAttackAnimationStart()
	{
		PlayerController.Instance.isAttacking = true;
	}
}
