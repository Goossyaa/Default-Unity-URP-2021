using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;



namespace LuxLWRPEssentials.Demo {
 
	public class ExtendedFlycam : MonoBehaviour
	{


	// slightly changed....
	 
		/*
		EXTENDED FLYCAM
			Desi Quintans (CowfaceGames.com), 17 August 2012.
			Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
	 
		LICENSE
			Free as in speech, and free as in beer.
	 
		FEATURES
			WASD/Arrows:    Movement
			          Q:    Dropp
			          E:    Climb
	                      Shift:    Move faster
	                    Control:    Move slower
	                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
		*/
	 
		public float cameraSensitivity = 90;
		public float climbSpeed = 4;
		public float normalMoveSpeed = 10;
		public float slowMoveFactor = 0.25f;
		public float fastMoveFactor = 3;
	 
		private float rotationX = 0.0f;
		private float rotationY = 0.0f;

		private bool isOrtho = false;
		private Camera cam;

		public bool isActive = true;
	 
		void Start () {
			rotationX = transform.eulerAngles.y;
			cam = GetComponent<Camera>();
			if (cam != null) {
				isOrtho = cam.orthographic;
			}
			
		}

	 	void Update ()
		{

			if (Keyboard.current.spaceKey.IsPressed()) {
				isActive = !isActive;
			}
			if (!isActive) {
				return;
			}

			// Cache deltaTime!
			var deltaTime = Time.deltaTime;	
			rotationX += Mouse.current.delta.x.ReadValue() * cameraSensitivity * deltaTime;
			rotationY += Mouse.current.delta.y.ReadValue() * cameraSensitivity * deltaTime;
			rotationY = Mathf.Clamp (rotationY, -90, 90);
	 
			var tempRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
			tempRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, tempRotation, deltaTime * 6.0f);
	 
		 	if (Keyboard.current.leftShiftKey.IsPressed() || Keyboard.current.rightShiftKey.IsPressed())
		 	{
				transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * (Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()) * deltaTime;
				transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * (Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue())  * deltaTime;
		 	}
		 	else if (Keyboard.current.leftCtrlKey.IsPressed() || Keyboard.current.rightCtrlKey.IsPressed())
		 	{
				transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * (Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()) * deltaTime;
				transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * (Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue()) * deltaTime;
		 	}
		 	else
		 	{
				if(isOrtho) {
					cam.orthographicSize *= (1.0f - (Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()) * deltaTime);
				}
				else {
					transform.position += transform.forward * normalMoveSpeed * (Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()) * deltaTime;
				}
				transform.position += transform.right * normalMoveSpeed * (Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue()) * deltaTime;
		 	}
	 
			if (Keyboard.current.qKey.IsPressed()) {transform.position -= transform.up * climbSpeed * deltaTime;}
			if (Keyboard.current.eKey.IsPressed()) {transform.position += transform.up * climbSpeed * deltaTime;}
		}
	}

}