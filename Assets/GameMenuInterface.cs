using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuInterface : MonoBehaviour
{
	public GameObject settingsCanvas;
	private bool settingsEnabled;
	private void Update()
	{
		if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			settingsEnabled = !settingsEnabled;
			settingsCanvas.gameObject.SetActive(settingsEnabled);
		}
	}
	public void OnClickContinue()
	{
		settingsEnabled = false;
	}
}
