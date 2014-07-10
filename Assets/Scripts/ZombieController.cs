using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	public float moveSpeed;
	public float turnSpeed;

	private Vector3 moveDirection;

	[SerializeField]
	private PolygonCollider2D[] colliders;
	private int currentColliderIndex = 0;

	// Use this for initialization
	void Start () {
		moveDirection = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * 1. Since you’ll be using the zombie’s current position a few times in this method, 
		 * you copy the position to a local variable.
		 */
		Vector3 currentPosition = transform.position;
		/*
		 * 2.Then you check to ensure the Fire1 button is currently pressed, 
		 * because you don’t want to calculate a new direction for the zombie otherwise. 
		 * See the upcoming note for more information about Input and Fire1.
		 */
		if (Input.GetButton("Fire1")) {
			/*
			 * 3.Using the scene’s main (and in this case, its only) Camera, 
			 * you convert the current mouse position to a world coordinate. 
			 * With an orthographic projection, 
			 * the z value in the position passed to ScreenToWorldPoint has no effect on the resulting x and y values,
			 * so here it’s safe to pass the mouse position directly.
			 */
			Vector3 moveToward = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			/*
			 * 4. You calculate the direction to move by subtracting the zombie’s current position from the target location. 
			 * Because you don’t want the zombie changing its position along the z-axis, 
			 * you set moveDirection‘s z value to 0, meaning, “Move zero units along the z-axis.” 
			 * Calling Normalize ensures moveDirection has a length of 1 (also known as “unit length”). 
			 * Unit length vectors are convenient because you can multiply them by a scalar value (like moveSpeed) 
			 * to make a vector pointing in the same direction, 
			 * but a certain length (like a moveSpeed-long vector pointing from the zombie 
			 * in the direction toward the mouse cursor). You’ll use this next.
			 */
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0;
			moveDirection.Normalize();
		}
		
		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);
		
		// angle
		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), turnSpeed * Time.deltaTime);

		EnforceBounds();
	}

	public void SetColliderForSprite(int spriteNum) {
		colliders[currentColliderIndex].enabled = false;
		currentColliderIndex = spriteNum;
		colliders[currentColliderIndex].enabled = true;
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Hit " + other.gameObject);
	}

	private void EnforceBounds () {
		Vector3 newPosition = transform.position;
		Camera mainCamera = Camera.main;
		Vector3 cameraPosition = mainCamera.transform.position;

		float xDist = mainCamera.aspect * mainCamera.orthographicSize;
		float xBounds = this.renderer.bounds.size.x / 2;
		xDist -= xBounds;
		float xMax = cameraPosition.x + xDist;
		float xMin = cameraPosition.x - xDist;

		if (newPosition.x < xMin || newPosition.x > xMax) {
			newPosition.x = Mathf.Clamp(newPosition.x, xMin, xMax);
			moveDirection.x = -moveDirection.x;
		}

		// TODO vertical bounds
		float yMax = mainCamera.orthographicSize - this.renderer.bounds.size.y / 2;

		if (newPosition.y < -yMax || newPosition.y > yMax) {
			newPosition.y = Mathf.Clamp(newPosition.y, -yMax, yMax);
			moveDirection.y = -moveDirection.y;
		}

		transform.position = newPosition;
	}
}
