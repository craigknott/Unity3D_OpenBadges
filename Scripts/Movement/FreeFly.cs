using UnityEngine;
using System.Collections;

/**
 * class FreeFly: Gives keyboard control to the user to move the main camera around.
 * Public variables control speed and sensitivity
 */
[AddComponentMenu("Camera-Control/Mouse Look")]
public class FreeFly : MonoBehaviour {

	/* Public Declarations */
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;

	public float moveSpeed = 10;

	public bool rightClick = false;

	/* Private Declarations */
	float rotationY = 0F;

	/**
	 * Default Update function, run every frame. Detects user keyboard input and moves the camera accordingly
	 * Params: None
	 * Returns: void
	 */
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Q)) {
			rightClick = !rightClick;
		} 
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKey (KeyCode.S)) {
			transform.Translate (Vector3.back * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKey (KeyCode.A)) {
			transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.D) || Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
		}
		if (rightClick) {
			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}
	}


	/**
	 * Default Start function, run at the initialisation of this object. Sets the object to not rotate
	 * Params: None
	 * Returns: void
	 */
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
}