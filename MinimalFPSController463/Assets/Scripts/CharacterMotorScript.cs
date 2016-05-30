﻿using UnityEngine;
using System.Collections;

//
// Ported to C# and modified by Richard Walsh
// May 30, 2016
// http://whilefun.com/
//

[RequireComponent(typeof(CharacterController))]

public class CharacterMotorScript : MonoBehaviour {

	[SerializeField]
	private bool canControl = true;
	[SerializeField]
	private bool useFixedUpdate = true;

	private Vector3 inputMoveDirection = Vector3.zero;
	private bool inputJump = false;

	// Crouching Stuff //
	[SerializeField]
	private bool crouchingEnabled = true;
	private bool isCrouching = false;
	private float standingHeight = 0.0f;
	private float crouchingHeight = 0.0f;
	private float previousCharacterHeight = 0.0f;

	[SerializeField]
	private bool runningEnabled = true;

	[SerializeField]
	private bool jumpingEnabled = true;

	// If set to false, player cannot move or crouch or jump
	[SerializeField]
	private bool canMove = true;

	[System.Serializable]
	public class CharacterMotorMovement {

		// The maximum horizontal speed when moving
		public float maxForwardSpeed = 6.0f;
		public float maxSidewaysSpeed = 6.0f;
		public float maxBackwardsSpeed = 6.0f;

		// Other speeds based on sit/stand/crouch states
		internal float walkingForwardSpeed = 6.0f;
		internal float walkingSidewaysSpeed = 6.0f;
		internal float walkingBackwardsSpeed = 6.0f;

		internal float runningForwardSpeed = 10.0f;
		internal float runningSidewaysSpeed = 10.0f;
		internal float runningBackwardsSpeed = 10.0f;

		internal float runningWhileCrouchingForwardSpeed = 5.0f;
		internal float runningWhileCrouchingSidewaysSpeed = 5.0f;
		internal float runningWhileCrouchingBackwardsSpeed = 5.0f;

		internal float walkingWhileCrouchingForwardSpeed = 3.0f;
		internal float walkingWhileCrouchingSidewaysSpeed = 3.0f;
		internal float walkingWhileCrouchingBackwardsSpeed = 3.0f;


		// Running Stuff //
		internal bool isRunning = false;
		[SerializeField]
		internal float runningMovementMultiplier = 1.5f;

		// Curve for multiplying speed based on slope (negative = downwards)
		public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90, 1), new Keyframe(0, 1), new Keyframe(90, 0));

		// How fast does the character change speeds?  Higher is faster.
		public float maxGroundAcceleration = 30.0f;
		public float maxAirAcceleration = 20.0f;
		// The gravity for the character
		public float gravity = 10.0f;
		public float maxFallSpeed = 20.0f;
		// The last collision flags returned from controller.Move
		internal CollisionFlags collisionFlags;
		// We will keep track of the character's current velocity,
		internal Vector3 velocity = Vector3.zero;
		// This keeps track of our current velocity while we're not grounded
		internal Vector3 frameVelocity = Vector3.zero;
		internal Vector3 hitPoint = Vector3.zero;
		internal Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);

	}

	[SerializeField]
	private CharacterMotorMovement movement = new CharacterMotorMovement();

	// NONE: The jump is not affected by velocity of floor at all.
	// INIT_TRANSFER: Jump gets its initial velocity from the floor, then gradualy comes to a stop.
	// PERMA_TRANSFER: Jump gets its initial velocity from the floor, and keeps that velocity until landing.
	// PERMA_LOCKED: Jump is relative to the movement of the last touched floor and will move together with that floor.
	public enum eMovementTransferOnJump { NONE, INIT_TRANSFER, PERMA_TRANSFER, PERMA_LOCKED };

	// We will contain all the jumping related variables in one helper class for clarity.
	[System.Serializable]
	public class CharacterMotorJumping {

		// How high do we jump when pressing jump and letting go immediately
		[SerializeField]
		internal float baseHeight = 1.0f;
		
		// We add extraHeight units (meters) on top when holding the button down longer while jumping
		[SerializeField]
		internal float extraHeight = 4.1f;
		
		// How much does the character jump out perpendicular to the surface on walkable surfaces?
		// 0 means a fully vertical jump and 1 means fully perpendicular.
		[SerializeField]
		internal float perpAmount = 0.0f;
		
		// How much does the character jump out perpendicular to the surface on too steep surfaces?
		// 0 means a fully vertical jump and 1 means fully perpendicular.
		[SerializeField]
		internal float steepPerpAmount = 0.5f;
		
		// Are we jumping? (Initiated with jump button and not grounded yet)
		// To see if we are just in the air (initiated by jumping OR falling) see the grounded variable.
		internal bool jumping = false;
		internal bool holdingJumpButton = false;
		
		// the time we jumped at (Used to determine for how long to apply extra jump power after jumping.)
		internal float lastStartTime  = 0.0f;
		internal float lastButtonDownTime  = -100.0f;
		internal Vector3 jumpDir = Vector3.up;

	}

	[SerializeField]
	private CharacterMotorJumping jumping = new CharacterMotorJumping();

	[System.Serializable]
	public class CharacterMotorMovingPlatform {

		[SerializeField]
		internal bool enabled = true;
		[SerializeField]
		internal eMovementTransferOnJump movementTransfer = eMovementTransferOnJump.PERMA_TRANSFER;
		internal Transform hitPlatform;
		internal Transform activePlatform;
		internal Vector3 activeLocalPoint;
		internal Vector3 activeGlobalPoint;
		internal Quaternion activeLocalRotation;
		internal Quaternion activeGlobalRotation;
		internal Matrix4x4 lastMatrix;
		internal Vector3 platformVelocity;
		internal bool newPlatform;

	}

	[SerializeField]
	private CharacterMotorMovingPlatform movingPlatform = new CharacterMotorMovingPlatform();

	[System.Serializable]
	public class CharacterMotorSliding {

		// Does the character slide on too steep surfaces?
		[SerializeField]
		internal bool enabled = true;
		// How fast does the character slide on steep surfaces?
		[SerializeField]
		internal float slidingSpeed = 15.0f;
		// How much can the player control the sliding direction?
		// If the value is 0.5 the player can slide sideways with half the speed of the downwards sliding speed.
		[SerializeField]
		internal float sidewaysControl = 1.0f;
		// How much can the player influence the sliding speed?
		// If the value is 0.5 the player can speed the sliding up to 150% or slow it down to 50%.
		[SerializeField]
		internal float speedControl = 0.4f;

	}

	[SerializeField]
	private CharacterMotorSliding sliding = new CharacterMotorSliding();

	private bool grounded = true;
	private Vector3 groundNormal = Vector3.zero;
	private Vector3 lastGroundNormal = Vector3.zero;
	private Transform tr;
	private CharacterController controller;
	
	void Awake(){

		controller = gameObject.GetComponent<CharacterController>();
		tr = gameObject.transform;

		standingHeight = controller.height;
		crouchingHeight = standingHeight/2.0f;

	}

	void Start(){
	
	}
	
	void UpdateFunction(){
	
		// We copy the actual velocity into a temporary variable that we can manipulate.
		Vector3 velocity = movement.velocity;
		
		// Update velocity based on input
		velocity = ApplyInputVelocityChange(velocity);
		
		// Apply gravity and jumping force
		velocity = ApplyGravityAndJumping(velocity);
		
		// Moving platform support
		Vector3 moveDistance = Vector3.zero;

		if(MoveWithPlatform()){

			Vector3 newGlobalPoint = movingPlatform.activePlatform.TransformPoint(movingPlatform.activeLocalPoint);
			moveDistance = (newGlobalPoint - movingPlatform.activeGlobalPoint);
			if(moveDistance != Vector3.zero){
				controller.Move(moveDistance);
			}
			
			// Support moving platform rotation as well:
			Quaternion newGlobalRotation = movingPlatform.activePlatform.rotation * movingPlatform.activeLocalRotation;
			Quaternion rotationDiff = newGlobalRotation * Quaternion.Inverse(movingPlatform.activeGlobalRotation);
			
			float yRotation = rotationDiff.eulerAngles.y;

			if(yRotation != 0){
				// Prevent rotation of the local up vector
				tr.Rotate(0, yRotation, 0);
			}

		}
		
		// Save lastPosition for velocity calculation.
		Vector3 lastPosition = tr.position;
		// We always want the movement to be framerate independent.  Multiplying by Time.deltaTime does this.
		Vector3 currentMovementOffset = velocity * Time.deltaTime;
		
		// Find out how much we need to push towards the ground to avoid loosing grouning
		// when walking down a step or over a sharp change in slope.
		float pushDownOffset = Mathf.Max(controller.stepOffset, new Vector3(currentMovementOffset.x, 0, currentMovementOffset.z).magnitude);

		if(grounded){
			currentMovementOffset -= pushDownOffset * Vector3.up;
		}
		
		// Reset variables that will be set by collision function
		movingPlatform.hitPlatform = null;
		groundNormal = Vector3.zero;
		
		// Move our character!
		movement.collisionFlags = controller.Move(currentMovementOffset);
		movement.lastHitPoint = movement.hitPoint;
		lastGroundNormal = groundNormal;
		
		if(movingPlatform.enabled && movingPlatform.activePlatform != movingPlatform.hitPlatform){

			if(movingPlatform.hitPlatform != null){

				movingPlatform.activePlatform = movingPlatform.hitPlatform;
				movingPlatform.lastMatrix = movingPlatform.hitPlatform.localToWorldMatrix;
				movingPlatform.newPlatform = true;

			}

		}
		
		// Calculate the velocity based on the current and previous position.  
		// This means our velocity will only be the amount the character actually moved as a result of collisions.
		Vector3 oldHVelocity = new Vector3(velocity.x, 0, velocity.z);
		movement.velocity = (tr.position - lastPosition) / Time.deltaTime;
		Vector3 newHVelocity = new Vector3(movement.velocity.x, 0, movement.velocity.z);
		
		// The CharacterController can be moved in unwanted directions when colliding with things.
		// We want to prevent this from influencing the recorded velocity.
		if(oldHVelocity == Vector3.zero){

			movement.velocity = new Vector3(0, movement.velocity.y, 0);

		}else{

			float projectedNewVelocity = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
			movement.velocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + movement.velocity.y * Vector3.up;

		}
		
		if(movement.velocity.y < velocity.y - 0.001){

			if(movement.velocity.y < 0){

				// Something is forcing the CharacterController down faster than it should.
				// Ignore this
				movement.velocity.y = velocity.y;

			}else{

				// The upwards movement of the CharacterController has been blocked.
				// This is treated like a ceiling collision - stop further jumping here.
				jumping.holdingJumpButton = false;

			}

		}
		
		// We were grounded but just loosed grounding
		if(grounded && !IsGroundedTest()){

			grounded = false;
			
			// Apply inertia from platform
			if(movingPlatform.enabled && (movingPlatform.movementTransfer == eMovementTransferOnJump.INIT_TRANSFER || movingPlatform.movementTransfer == eMovementTransferOnJump.PERMA_TRANSFER)){

				movement.frameVelocity = movingPlatform.platformVelocity;
				movement.velocity += movingPlatform.platformVelocity;

			}
			
			SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);

			// We pushed the character down to ensure it would stay on the ground if there was any.
			// But there wasn't so now we cancel the downwards offset to make the fall smoother.
			tr.position += pushDownOffset * Vector3.up;

		}else if(!grounded && IsGroundedTest()){

			// We were not grounded but just landed on something
			grounded = true;
			jumping.jumping = false;
			SubtractNewPlatformVelocity();
			
			SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);

		}
		
		// Moving platforms support
		if(MoveWithPlatform()){

			// Use the center of the lower half sphere of the capsule as reference point.
			// This works best when the character is standing on moving tilting platforms. 
			movingPlatform.activeGlobalPoint = tr.position + Vector3.up * (float)(controller.center.y - controller.height*0.5 + controller.radius);
			movingPlatform.activeLocalPoint = movingPlatform.activePlatform.InverseTransformPoint(movingPlatform.activeGlobalPoint);
			
			// Support moving platform rotation as well:
			movingPlatform.activeGlobalRotation = tr.rotation;
			movingPlatform.activeLocalRotation = Quaternion.Inverse(movingPlatform.activePlatform.rotation) * movingPlatform.activeGlobalRotation; 
		
		}

		if(isCrouching){

			previousCharacterHeight = controller.height;
			gameObject.GetComponent<CharacterController>().height = Mathf.Lerp(gameObject.GetComponent<CharacterController>().height, crouchingHeight, 5*Time.deltaTime);
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (controller.height - previousCharacterHeight)/2 ,gameObject.transform.position.z);

		}else{

			previousCharacterHeight = controller.height;

			// Only get taller (i.e. stand up) if there is room above us to do so
			if(haveHeadRoomToStand()){
				gameObject.GetComponent<CharacterController>().height = Mathf.Lerp(gameObject.GetComponent<CharacterController>().height, standingHeight, 5*Time.deltaTime);
			}

			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (controller.height - previousCharacterHeight)/2 ,gameObject.transform.position.z);

		}


	}//UpdateFunction

	void FixedUpdate(){

		if(movingPlatform.enabled){

			if(movingPlatform.activePlatform != null){

				if(!movingPlatform.newPlatform){

					//Vector3 lastVelocity = movingPlatform.platformVelocity;
					movingPlatform.platformVelocity = (movingPlatform.activePlatform.localToWorldMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint) - movingPlatform.lastMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint)) / Time.deltaTime;
				
				}

				movingPlatform.lastMatrix = movingPlatform.activePlatform.localToWorldMatrix;
				movingPlatform.newPlatform = false;

			}else{

				movingPlatform.platformVelocity = Vector3.zero;	

			}

		}
		
		if(useFixedUpdate){
			UpdateFunction();
		}

	}

	void Update(){

		if(!useFixedUpdate){
			UpdateFunction();
		}

	}

	private Vector3 ApplyInputVelocityChange(Vector3 velocity){

		if(!canControl){
			inputMoveDirection = Vector3.zero;
		}
		
		// Find desired velocity
		Vector3 desiredVelocity;

		if(grounded && TooSteep()){

			// The direction we're sliding in
			desiredVelocity = new Vector3(groundNormal.x, 0, groundNormal.z).normalized;
			// Find the input movement direction projected onto the sliding direction
			Vector3 projectedMoveDir = Vector3.Project(inputMoveDirection, desiredVelocity);
			// Add the sliding direction, the spped control, and the sideways control vectors
			desiredVelocity = desiredVelocity + projectedMoveDir * sliding.speedControl + (inputMoveDirection - projectedMoveDir) * sliding.sidewaysControl;
			// Multiply with the sliding speed
			desiredVelocity *= sliding.slidingSpeed;

		}else{

			desiredVelocity = GetDesiredHorizontalVelocity();

		}
		
		if(movingPlatform.enabled && movingPlatform.movementTransfer == eMovementTransferOnJump.PERMA_TRANSFER){

			desiredVelocity += movement.frameVelocity;
			desiredVelocity.y = 0;

		}
		
		if(grounded){

			desiredVelocity = AdjustGroundVelocityToNormal(desiredVelocity, groundNormal);

		}else{

			velocity.y = 0.0f;

		}
		
		// Enforce max velocity change
		float maxVelocityChange = GetMaxAcceleration(grounded) * Time.deltaTime;
		Vector3 velocityChangeVector = (desiredVelocity - velocity);

		if(velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange){

			velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;

		}

		// If we're in the air and don't have control, don't apply any velocity change at all.
		// If we're on the ground and don't have control we do apply it - it will correspond to friction.
		if(grounded || canControl){

			velocity += velocityChangeVector;

		}
		
		if(grounded){

			// When going uphill, the CharacterController will automatically move up by the needed amount.
			// Not moving it upwards manually prevent risk of lifting off from the ground.
			// When going downhill, DO move down manually, as gravity is not enough on steep hills.
			velocity.y = Mathf.Min(velocity.y, 0);

		}
		
		return velocity;

	}

	private Vector3 ApplyGravityAndJumping(Vector3 velocity){
		
		if(!inputJump || !canControl){
			jumping.holdingJumpButton = false;
			jumping.lastButtonDownTime = -100;
		}
		
		if(inputJump && jumping.lastButtonDownTime < 0 && canControl){
			jumping.lastButtonDownTime = Time.time;
		}
		
		if(grounded){

			velocity.y = Mathf.Min(0, velocity.y) - movement.gravity * Time.deltaTime;

		}else{

			velocity.y = movement.velocity.y - movement.gravity * Time.deltaTime;
			
			// When jumping up we don't apply gravity for some time when the user is holding the jump button.
			// This gives more control over jump height by pressing the button longer.
			if(jumping.jumping && jumping.holdingJumpButton){

				// Calculate the duration that the extra jump force should have effect.
				// If we're still less than that duration after the jumping time, apply the force.
				if(Time.time < jumping.lastStartTime + jumping.extraHeight / CalculateJumpVerticalSpeed(jumping.baseHeight)){

					// Negate the gravity we just applied, except we push in jumpDir rather than jump upwards.
					velocity += jumping.jumpDir * movement.gravity * Time.deltaTime;

				}

			}
			
			// Make sure we don't fall any faster than maxFallSpeed. This gives our character a terminal velocity.
			velocity.y = Mathf.Max (velocity.y, -movement.maxFallSpeed);

		}
		
		if(grounded){

			// Jump only if the jump button was pressed down in the last 0.2 seconds.
			// We use this check instead of checking if it's pressed down right now
			// because players will often try to jump in the exact moment when hitting the ground after a jump
			// and if they hit the button a fraction of a second too soon and no new jump happens as a consequence,
			// it's confusing and it feels like the game is buggy.
			if(jumpingEnabled && canControl && (Time.time - jumping.lastButtonDownTime < 0.2)){

				grounded = false;
				jumping.jumping = true;
				jumping.lastStartTime = Time.time;
				jumping.lastButtonDownTime = -100;
				jumping.holdingJumpButton = true;
				
				// Calculate the jumping direction
				if(TooSteep()){

					jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.steepPerpAmount);

				}else{

					jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.perpAmount);

				}

				// Apply the jumping force to the velocity. Cancel any vertical velocity first.
				velocity.y = 0;
				velocity += jumping.jumpDir * CalculateJumpVerticalSpeed(jumping.baseHeight);
				
				// Apply inertia from platform
				if(movingPlatform.enabled && (movingPlatform.movementTransfer == eMovementTransferOnJump.INIT_TRANSFER || movingPlatform.movementTransfer == eMovementTransferOnJump.PERMA_TRANSFER)){

					movement.frameVelocity = movingPlatform.platformVelocity;
					velocity += movingPlatform.platformVelocity;

				}
				
				SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);

			}else{

				jumping.holdingJumpButton = false;

			}

		}
		
		return velocity;

	}



	void OnControllerColliderHit(ControllerColliderHit hit){

		if(hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0){

			if((hit.point - movement.lastHitPoint).sqrMagnitude > 0.001 || lastGroundNormal == Vector3.zero){
				groundNormal = hit.normal;
			}else{
				groundNormal = lastGroundNormal;
			}
			
			movingPlatform.hitPlatform = hit.collider.transform;
			movement.hitPoint = hit.point;
			movement.frameVelocity = Vector3.zero;

		}

	}

	private IEnumerator SubtractNewPlatformVelocity(){

		// When landing, subtract the velocity of the new ground from the character's velocity
		// since movement in ground is relative to the movement of the ground.
		if(movingPlatform.enabled && (movingPlatform.movementTransfer == eMovementTransferOnJump.INIT_TRANSFER || movingPlatform.movementTransfer == eMovementTransferOnJump.PERMA_TRANSFER)){

			// If we landed on a new platform, we have to wait for two FixedUpdates
			// before we know the velocity of the platform under the character
			if(movingPlatform.newPlatform){

				Transform platform = movingPlatform.activePlatform;

				yield return new WaitForFixedUpdate();
				yield return new WaitForFixedUpdate();

				if(grounded && platform == movingPlatform.activePlatform){
					yield break;
				}

			}

			movement.velocity -= movingPlatform.platformVelocity;

		}

	}

	private bool MoveWithPlatform(){
		return(movingPlatform.enabled && (grounded || movingPlatform.movementTransfer == eMovementTransferOnJump.PERMA_LOCKED) && movingPlatform.activePlatform != null);
	}

	private Vector3 GetDesiredHorizontalVelocity(){

		// Find desired velocity
		Vector3 desiredLocalDirection = tr.InverseTransformDirection(inputMoveDirection);
		float maxSpeed = MaxSpeedInDirection(desiredLocalDirection);

		if(grounded){

			// Modify max speed on slopes based on slope speed multiplier curve
			var movementSlopeAngle = Mathf.Asin(movement.velocity.normalized.y)  * Mathf.Rad2Deg;
			maxSpeed *= movement.slopeSpeedMultiplier.Evaluate(movementSlopeAngle);

		}

		return tr.TransformDirection(desiredLocalDirection * maxSpeed);

	}

	private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal){

		Vector3 sideways = Vector3.Cross(Vector3.up, hVelocity);
		return Vector3.Cross(sideways, groundNormal).normalized * hVelocity.magnitude;

	}

	private bool IsGroundedTest(){
		return (groundNormal.y > 0.01);
	}
	
	// Maximum acceleration on ground and in air
	private float GetMaxAcceleration(bool grounded){

		if(grounded){
			return movement.maxGroundAcceleration;
		}else{
			return movement.maxAirAcceleration;
		}

	}

	private float CalculateJumpVerticalSpeed(float targetJumpHeight){

		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * movement.gravity);

	}
	
	private bool IsJumping(){
		return jumping.jumping;
	}
	
	private bool IsSliding(){
		return (grounded && sliding.enabled && TooSteep());
	}
	
	private bool IsTouchingCeiling(){
		return(movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
	}
	
	private bool IsGrounded(){
		return grounded;
	}
	
	private bool TooSteep(){
		return (groundNormal.y <= Mathf.Cos(controller.slopeLimit * Mathf.Deg2Rad));
	}
	
	private Vector3 GetDirection(){
		return inputMoveDirection;
	}
	
	private void SetControllable(bool controllable){
		canControl = controllable;
	}

	// Project a direction onto elliptical quater segments based on forward, sideways, and backwards speed.
	// The function returns the length of the resulting vector.
	private float MaxSpeedInDirection(Vector3 desiredMovementDirection){

		if(desiredMovementDirection == Vector3.zero){
			return 0;
		}else{
			float zAxisEllipseMultiplier = (desiredMovementDirection.z > 0 ? movement.maxForwardSpeed : movement.maxBackwardsSpeed) / movement.maxSidewaysSpeed;
			Vector3 temp = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
			float length = new Vector3(temp.x, 0, temp.z * zAxisEllipseMultiplier).magnitude * movement.maxSidewaysSpeed;
			return length;
		}

	}

	private void SetVelocity(Vector3 velocity){

		grounded = false;
		movement.velocity = velocity;
		movement.frameVelocity = Vector3.zero;
		SendMessage("OnExternalVelocity");

	}

	// Other custom functions //

	public void setJumpInput(bool jump){

		if(canMove){
			inputJump = jump;
		}else{
			inputJump = false;
		}

	}

	public void setInputMoveDirection(Vector3 dir){

		if(canMove){
			inputMoveDirection = dir;
		}else{
			inputMoveDirection = Vector3.zero;
		}

	}

	public void toggleCrouch(){

		if(crouchingEnabled && canMove){

			if(isCrouching){
				isCrouching = false;
			}else{
				isCrouching = true;
			}

		}

	}

	private bool haveHeadRoomToStand(){

		bool haveHeadRoom = true;
		//Debug.DrawRay(gameObject.transform.position, gameObject.transform.up * (standingHeight - (controller.height/2.0f)), Color.red);

		RaycastHit headRoomHit;
		if(Physics.Raycast(gameObject.transform.position, gameObject.transform.up, out headRoomHit, (standingHeight - (controller.height/2.0f)))){
			//Debug.Log("Headroom hit " + headRoomHit.collider.transform.name);
			haveHeadRoom = false;
		}

		return haveHeadRoom;

	}

	public bool isCurrentlyRunning(){
		return movement.isRunning;
	}

	public bool isCurrentlyCrouching(){
		return isCrouching;
	}

	public void setRunning(bool running){

		if(runningEnabled){

			movement.isRunning = running;

			if(running){

				if(isCrouching){

					movement.maxForwardSpeed = movement.runningWhileCrouchingForwardSpeed;
					movement.maxSidewaysSpeed = movement.runningWhileCrouchingSidewaysSpeed;
					movement.maxBackwardsSpeed = movement.runningWhileCrouchingBackwardsSpeed;

				}else{

					movement.maxForwardSpeed = movement.runningForwardSpeed;
					movement.maxSidewaysSpeed = movement.runningSidewaysSpeed;
					movement.maxBackwardsSpeed = movement.runningBackwardsSpeed;

				}

			}else{

				if(isCrouching){

					movement.maxForwardSpeed = movement.walkingWhileCrouchingForwardSpeed;
					movement.maxSidewaysSpeed = movement.walkingWhileCrouchingSidewaysSpeed;
					movement.maxBackwardsSpeed = movement.walkingWhileCrouchingBackwardsSpeed;

				}else{

					movement.maxForwardSpeed = movement.walkingForwardSpeed;
					movement.maxSidewaysSpeed = movement.walkingSidewaysSpeed;
					movement.maxBackwardsSpeed = movement.walkingBackwardsSpeed;

				}

			}

		}

	}

	public void enableMovement(){
		canMove = true;
	}

	public void disableMovement(){
		canMove = false;
	}

	public bool canCurrentlyMove(){
		return canMove;
	}

	public void enableLook(){
		gameObject.GetComponent<MouseLookScript>().enableLook();
	}

	public void disableLook(){
		gameObject.GetComponent<MouseLookScript>().disableLook();
	}

	public bool canCurrentlyLook(){
		return gameObject.GetComponent<MouseLookScript>().canCurrentlyLook();
	}

}
