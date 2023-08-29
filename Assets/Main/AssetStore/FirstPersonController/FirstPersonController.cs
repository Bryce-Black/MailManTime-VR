// CHANGE LOG
// 
// CHANGES || version VERSION
//
// "Enable/Disable Headbob, Changed look rotations - should result in reduced camera jitters" || version 1.0.1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


#if UNITY_EDITOR
using UnityEditor;
    using System.Net;
#endif

public class FirstPersonController : MonoBehaviour
{
    public GameObject numberTextKey;
    public GameObject numberTextMail;

    public GameObject reticle;
    public Animator poofAnimation;
    public TextMeshProUGUI screenInfoText;
    public Animator scrennInfoAnim;
    private Rigidbody rb;
    #region ShootingVariables
    public GameObject shootLocation;
    private Transform shootingPoint;
    private float letterSpeed = 5000f;
    private string letterNameInResourcesFolder = "MailMail";
    private string keyNameInResourceFolder = "Key";
    private bool keyInHand = true;
    private Vector3 currentVector3;
    #endregion ShootingVariables

    #region UI
    public GameObject keySelectHighlight;
    public List<GameObject> keyUIGameObjects;
    private Vector3 selectedKeyPositionV3;
    private int keySelectIndexInt = 0;

    public GameObject mailSelectHighlight;
    public List<GameObject> mailUIGameObjects;
    private Vector3 selectedMailPositionV3;
    private int mailSelectIndexInt = 0;

    #endregion UI
    #region Camera Movement Variables

    public Camera playerCamera;
    
    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;
    public float maxLookAngleLower = 50f;
    public float maxLookAngleHigher = 50f;
    // Crosshair
    public bool lockCursor = true;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;
    public float xRotation = 0f;
    public float yRotation = -90f;
    public float zRotation = 0f;
    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;


    public KeyCode pauseButton = KeyCode.Joystick1Button7;
    // Internal Variables
    private bool isZoomed = false;

    #endregion
    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    // Internal Variables
    private bool isWalking = false;

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    // Sprint Bar
    public bool useSprintBar = true;
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;

    // Internal Variables
    private CanvasGroup sprintBarCG;
    private bool isSprinting = false;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    #endregion

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Joystick1Button0;
    public float jumpPower = 5f;

    // Internal Variables
    public bool isGrounded = false;
    private IEnumerator powerUp;
    private bool speedMofiied = false;
    private bool jumpModified = false;
    private MailBoxContoller mbController;
    public PowerUpController puController;
    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion
    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;
    public float gravityScale = 1f;

    private bool clippingControl = false;
    private Vector3 newPosition;
    private Vector3 cameraHolderVector3;
    public float raycastVector3x = 0;
    public float raycastVector3y = 0;
    public float raycastVector3z = 0;
    public GameObject rayCastAnchorGameObject;
    public GameObject rayCastCameraAnchorGameObject;
    #endregion

    public InputActionProperty rightTriggerPull;
    public InputActionProperty rightSqueezePull;
    public InputActionProperty rightJoystickAxis;
    //public InputActionProperty rightAButton;
    //public InputActionProperty rightBButton;

    public InputActionProperty leftTriggerPull;
    public InputActionProperty leftSqueezePull;
    public InputActionProperty leftJoystickAxis;
    //public InputActionProperty leftAButton;
    //public InputActionProperty leftBButton;
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;
    public UIManager uImanager;

    private void Awake()
    {
        selectedKeyPositionV3 = keyUIGameObjects[0].transform.position;
        selectedMailPositionV3 = mailUIGameObjects[0].transform.position;
        
        rb = GetComponent<Rigidbody>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shootingPoint = playerCamera.transform;
        crosshairObject = GetComponentInChildren<Image>();

        // Set internal variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;

        if (!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;
            sprintCooldownReset = sprintCooldown;
        }
    }

    void Start()
    {

        Application.targetFrameRate = 120;
        Time.fixedDeltaTime = 0.00833f;
        mbController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        #region Sprint Bar
        sprintBarCG = GetComponentInChildren<CanvasGroup>();

        //if(useSprintBar)
        //{
        //    sprintBarBG.gameObject.SetActive(true);
        //    sprintBar.gameObject.SetActive(true);

        //    float screenWidth = Screen.width;
        //    float screenHeight = Screen.height;

        //    sprintBarWidth = screenWidth * sprintBarWidthPercent;
        //    sprintBarHeight = screenHeight * sprintBarHeightPercent;

        //    sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
        //    sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

        //    if(hideBarWhenFull)
        //    {
        //        sprintBarCG.alpha = 0;
        //    }
        //}
        //else
        //{
        //    sprintBarBG.gameObject.SetActive(false);
        //    sprintBar.gameObject.SetActive(false);
        //}

        #endregion
    }
    public void ScreenInfoActivate(string infoText)
    {
        screenInfoText.text = infoText;
        scrennInfoAnim.SetTrigger("ScreenInfoTrigger");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            screenInfoText.text = "Speed Boost! 10 Seconds";
            scrennInfoAnim.SetTrigger("ScreenInfoTrigger");
            Debug.Log("Speedboost");
            if(powerUp != null)
            {
                StopCoroutine(powerUp);
            }
            else
            {
                speedMofiied = true;
                powerUp = PowerUp(10f);
                walkSpeed *= 2;
                sprintSpeed *= 2;
                StartCoroutine(powerUp);
            }
            Destroy(other.gameObject);
            //puController.GenerateRandomSpawnLocation();
            
        }
        if (other.gameObject.tag == "JumpBoost")
        {
            screenInfoText.text = "Jump Boost! 10 Seconds";
            scrennInfoAnim.SetTrigger("ScreenInfoTrigger");
            Debug.Log("JumpBoost");
            if (powerUp != null)
            {
                StopCoroutine(powerUp);
            }
            else
            {
                jumpModified = true;
                jumpPower *= 2;
                powerUp = PowerUp(10f);
                StartCoroutine(powerUp);
            }
            Destroy(other.gameObject);
            //puController.GenerateRandomSpawnLocation();

        }
        if (other.gameObject.tag == "TimeAdd")
        {
            screenInfoText.text = "+5 Seconds!";
            scrennInfoAnim.SetTrigger("ScreenInfoTrigger");
            Debug.Log("TimeAdd");
            mbController.TimeResetPowerUp();
            Destroy(other.gameObject);
            puController.GenerateRandomSpawnLocation();
        }
        if (other.gameObject.tag == "LaunchPad")
        {
            Debug.Log("LaunchPad");
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * 5000f);
        }
    }
    private IEnumerator PowerUp(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(speedMofiied)
        {
            walkSpeed /= 2;
            sprintSpeed /= 2;
            speedMofiied = false;
        }
        if(jumpModified)
        {
            jumpPower /= 2;
            jumpModified = false;
        }
        Debug.Log("PowerUpDone");

    }

    float camRotation;

    private void Shoot()
    {
        
        if(!mbController.gameIsPaused)
        {
            poofAnimation.SetTrigger("Shoot");
            if (keyInHand)
            {

                // Instantiate the projectile at the camera's position and rotation
                GameObject projectile = Instantiate(Resources.Load<GameObject>(keyNameInResourceFolder), shootLocation.transform.position, shootLocation.transform.rotation);
                projectile.transform.Rotate(0, 90, 0, Space.World);
                // Get the direction the player is pointing
                Vector3 direction = shootingPoint.forward;
                
                // Add force to the projectile if it has a Rigidbody component
                Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
                if (projectileRigidbody != null)
                {
                    //Debug.Log("direction is x:" + direction.x + "y:" + direction.y + "z: " + direction.z);
                    projectileRigidbody.AddForce(direction * 5500f);
                    projectileRigidbody.AddForce(Vector3.up * 100f);
                }
            }
            else
            {
                // Instantiate the projectile at the camera's position and rotation
                currentVector3 = shootLocation.transform.position;
                GameObject projectile = Instantiate(Resources.Load<GameObject>(letterNameInResourcesFolder), shootLocation.transform.position, shootLocation.transform.rotation);
                projectile.transform.position = currentVector3;
                projectile.transform.Rotate(xRotation, yRotation, zRotation, Space.World);
                // Get the direction the player is pointing
                Vector3 direction = shootingPoint.forward;
                // Add force to the projectile if it has a Rigidbody component
                Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
                if (projectileRigidbody != null)
                {
                    //Debug.Log("direction is x:" + direction.x + "y:" + direction.y + "z: " + direction.z);
                    projectileRigidbody.AddForce(direction * letterSpeed);
                    projectileRigidbody.AddForce(Vector3.up * 100f);
                    float turn = Input.GetAxis("Horizontal");
                    projectileRigidbody.AddTorque(5000f * turn * projectileRigidbody.transform.right);
                }
            }
        }
        
    }
    public void UpdateKeyName(string keyName)
    {
        keyNameInResourceFolder = keyName;
    }
    public void UpdateMailName(string mailName)
    {
        letterNameInResourcesFolder = mailName;
    }
    private void Update()
    {
        float rightTriggerValue = rightTriggerPull.action.ReadValue<float>();
        float leftTriggerValue = leftTriggerPull.action.ReadValue<float>();
        Vector2 turnValue = rightJoystickAxis.action.ReadValue<Vector2>();
        //Debug.Log("turnValue: " + turnValue);
        if(rightTriggerValue > 0)
        {
            //Debug.Log("trig value: " + rightTriggerValue);
            rightHandAnimator.SetFloat("Trigger", 1);
        }
        else
        {
            rightHandAnimator.SetFloat("Trigger", 0);
        }

        float rightSqueezeValuetemp = rightSqueezePull.action.ReadValue<float>();
        if(rightSqueezeValuetemp > 0)
        {
            rightHandAnimator.SetFloat("Grip", 1);
        }
        else
        {
            rightHandAnimator.SetFloat("Grip", 0);
        }

        float leftSqueezeValuetemp = leftSqueezePull.action.ReadValue<float>();
        if (leftSqueezeValuetemp > 0)
        {
            leftHandAnimator.SetFloat("Grip", 1);
        }
        else
        {
            leftHandAnimator.SetFloat("Grip", 0);
        }


        if (turnValue.x == 0 || turnValue.y == 0)
        {
            //do nothing
        }
        else
        {
            yaw = transform.localEulerAngles.y + turnValue.x * mouseSensitivity;
            transform.localEulerAngles = new Vector3(0, yaw, 0);
        }
        if (Input.GetKeyDown(jumpKey))
        {
            if (enableJump)
            {
                CheckGround();
                if (isGrounded)
                {
                    Debug.Log("A button pressed, Jump");
                    Jump();
                }

            }
        }
        if (Input.GetButtonDown("BButton"))
        {

        }
        if (Input.GetButtonDown("XButton"))
        {

        }
        if (Input.GetButtonDown("YButton"))
        {

        }

        if(Input.GetKeyUp(pauseButton))
        {
            Debug.Log("pauseButtonPressed");
            uImanager.ToggleOptionsMenu();
        }


        #region Camera Zoom
        //if (leftTriggerValue > 0)
        //{
        //    Debug.Log("Zoomy bam boomy");
        //    //reticle.SetActive(true);
        //    playerCamera.fieldOfView = 40;
        //    //if (Input.GetKey(zoomKey))
        //    //{
                

        //    //}
        //    //else
        //    //{
        //    //    //reticle.SetActive(false);
        //    //}
        //}
        //else
        //{
        //    playerCamera.fieldOfView = 60;

        //}




        #endregion

        #region Sprint

        if (enableSprint)
        {
            if (isSprinting)
            {
                isZoomed = false;
                //playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);

                // Drain sprint remaining while sprinting
                if (!unlimitedSprint)
                {
                    sprintRemaining -= 1 * Time.deltaTime;
                    if (sprintRemaining <= 0)
                    {
                        isSprinting = false;
                        isSprintCooldown = true;
                    }
                }
            }
            else
            {
                //Regain sprint while not sprinting
                sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
            }

            //Handles sprint cooldown
            //When sprint remaining == 0 stops sprint ability until hitting cooldown
            if (isSprintCooldown)
            {
                sprintCooldown -= 1 * Time.deltaTime;
                if (sprintCooldown <= 0)
                {
                    isSprintCooldown = false;
                }
            }
            else
            {
                sprintCooldown = sprintCooldownReset;
            }

            // Handles sprintBar 
            if (useSprintBar && !unlimitedSprint)
            {
                float sprintRemainingPercent = sprintRemaining / sprintDuration;
                sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
            }
        }

        #endregion

        #region Jump

        // Gets input and calls jump method
        

        #endregion

        #region Crouch

        //if (enableCrouch)
        //{
        //    if(Input.GetKeyDown(crouchKey) && !holdToCrouch)
        //    {
        //        Crouch();
        //    }

        //    if(Input.GetKeyDown(crouchKey) && holdToCrouch)
        //    {
        //        isCrouched = false;
        //        Crouch();
        //    }
        //    else if(Input.GetKeyUp(crouchKey) && holdToCrouch)
        //    {
        //        isCrouched = true;
        //        Crouch();
        //    }
        //}

        #endregion

    }

    void FixedUpdate()
    {
        //Vector3 tempPlayerRayVector3 = rayCastAnchorGameObject.transform.position;
        //Vector3 tempCameraRayVector3 = rayCastCameraAnchorGameObject.transform.position;

        //tempPlayerRayVector3.x += raycastVector3x;
        //tempPlayerRayVector3.y += raycastVector3y;
        //tempPlayerRayVector3.z += raycastVector3z;

        //Ray playerAnchoredBackRay = new Ray(tempPlayerRayVector3, -rayCastAnchorGameObject.transform.forward);
        //Debug.DrawRay(playerAnchoredBackRay.origin, playerAnchoredBackRay.direction * 5f, Color.red);
        //RaycastHit hit3;

        //Ray cameraAnchoredBackRay = new Ray(tempCameraRayVector3, -rayCastCameraAnchorGameObject.transform.forward);
        //Debug.DrawRay(cameraAnchoredBackRay.origin, cameraAnchoredBackRay.direction * 5f, Color.red);
        //RaycastHit cameraAnchorHit;


        //// Perform the raycast
        //if ((Physics.Raycast(playerAnchoredBackRay, out hit3, 5f)))
        //{
        //    // Check if the ray hit a collider with a tag]

        //    newPosition = playerCamera.transform.localPosition;
        //    if (newPosition.z < 3f)
        //    {
        //        newPosition.z += .1f;
        //        playerCamera.transform.localPosition = newPosition;
        //    }
        //    //if (!clippingControl)
        //    //{
        //    //    newPosition = playerCamera.transform.localPosition;
        //    //    newPosition.z = 3f;
        //    //    playerCamera.transform.localPosition = newPosition;
        //    //    clippingControl = true;
        //    //    Debug.Log("newPosition" + newPosition);
        //    //}
        //}
        //else
        //{
        //    if ((Physics.Raycast(playerAnchoredBackRay, out hit3, 9f)))
        //    {
        //    }
        //    else
        //    {
        //        newPosition = playerCamera.transform.localPosition;
        //        if (newPosition.z > -3f)
        //        {
        //            newPosition.z -= .1f;
        //            playerCamera.transform.localPosition = newPosition;
        //        }
        //        //if (clippingControl)
        //        //{
        //        //    newPosition = playerCamera.transform.localPosition;
        //        //    newPosition.z = -3f;
        //        //    playerCamera.transform.localPosition = newPosition;
        //        //    clippingControl = false;
        //        //}
        //    }

        //}


        //if ((Physics.Raycast(cameraAnchoredBackRay, out cameraAnchorHit, 5f)))
        //{
        //    // Check if the ray hit a collider with a tag]

        //    newPosition = playerCamera.transform.localPosition;
        //    if (newPosition.z < 3f)
        //    {
        //        newPosition.z += .1f;
        //        playerCamera.transform.localPosition = newPosition;
        //    }
        //    //if (!clippingControl)
        //    //{
        //    //    newPosition = playerCamera.transform.localPosition;
        //    //    newPosition.z = 3f;
        //    //    playerCamera.transform.localPosition = newPosition;
        //    //    clippingControl = true;
        //    //    Debug.Log("newPosition" + newPosition);
        //    //}
        //}
        //else
        //{
        //    if ((Physics.Raycast(cameraAnchoredBackRay, out cameraAnchorHit, 9f)))
        //    {
        //    }
        //    else
        //    {
        //        newPosition = playerCamera.transform.localPosition;
        //        if (newPosition.z > -3f)
        //        {
        //            newPosition.z -= .1f;
        //            playerCamera.transform.localPosition = newPosition;
        //        }
        //        //if (clippingControl)
        //        //{
        //        //    newPosition = playerCamera.transform.localPosition;
        //        //    newPosition.z = -3f;
        //        //    playerCamera.transform.localPosition = newPosition;
        //        //    clippingControl = false;
        //        //}
        //    }

        //}

        //GRAVITY
        rb.AddForce(Vector3.down * gravityScale);
        #region Movement
        if (playerCanMove)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Debug.Log("target velocity?:" + targetVelocity);
            if (targetVelocity.x == 0 && targetVelocity.z == 0)
            {

                CheckGround();
                //if (isGrounded)
                //{
                //    rb.velocity = Vector3.zero;
                //    rb.Sleep();
                //}


                //do nothing
            }
            else
            {

                ////If we want to adjust air manipulation
                

                if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
                {
                    isWalking = true;
                }
                else
                {
                    isWalking = false;
                }


                // All movement calculations shile sprint is active
                if (enableSprint && Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown)
                {
                    targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                    // Apply a force that attempts to reach our target velocity
                    Vector3 velocity = rb.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    velocityChange.y = 0;
                    // Player is only moving when velocity change != 0
                    // Makes sure fov change only happens during movement
                    if (velocityChange.x != 0 || velocityChange.z != 0)
                    {
                        isSprinting = true;

                        //if (isCrouched)
                        //{
                        //    Crouch();
                        //}

                        if (hideBarWhenFull && !unlimitedSprint)
                        {
                            //sprintBarCG.alpha += 5 * Time.deltaTime;
                        }
                    }

                    rb.AddForce(velocityChange, ForceMode.VelocityChange);
                }
                // All movement calculations while walking
                else
                {
                    isSprinting = false;

                    if (hideBarWhenFull && sprintRemaining == sprintDuration)
                    {
                        //sprintBarCG.alpha -= 3 * Time.deltaTime;
                    }

                    targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
                    //Apply a force that attempts to reach our target velocity
                    Vector3 velocity = rb.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    velocityChange.y = 0;

                    rb.AddForce(velocityChange, ForceMode.VelocityChange);

                }
            }
            // Checks if player is walking and isGrounded
            // Will allow head bob
            
        }

        #endregion
    }
    public void ChangeMailInfo(string newLetterName, float newSpeed)
    {
        letterNameInResourcesFolder = newLetterName;
    }
    public void ChangeKeyInfo(string newKeyName)
    {
        keyNameInResourceFolder = newKeyName;
    }
    public void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .45f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //Debug.Log("Is grounded?: " + isGrounded);
    }

    private void Jump()
    {
        // Adds force to the player rigidbody to jump

        rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
        isGrounded = false;
        //Debug.Log("jumped");
        // When crouched and using toggle system, will uncrouch for a jump
        //if (isCrouched && !holdToCrouch)
        //{
        //    Crouch();
        //}
    }

    //private void Crouch()
    //{
    //// Stands player up to full height
    //// Brings walkSpeed back up to original speed
    //if (isCrouched)
    //{
    //    transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
    //    walkSpeed /= speedReduction;

    //    isCrouched = false;
    //}
    //// Crouches player down to set height
    //// Reduces walkSpeed
    //else
    //{
    //    transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
    //    walkSpeed *= speedReduction;

    //    isCrouched = true;
    //}
    //}

    //    private void HeadBob()
    //    {
    //        if(isWalking)
    //        {
    //            // Calculates HeadBob speed during sprint
    //            if(isSprinting)
    //            {
    //                timer += Time.deltaTime * (bobSpeed + sprintSpeed);
    //            }
    //            // Calculates HeadBob speed during crouched movement
    //            else if (isCrouched)
    //            {
    //                timer += Time.deltaTime * (bobSpeed * speedReduction);
    //            }
    //            // Calculates HeadBob speed during walking
    //            else
    //            {
    //                timer += Time.deltaTime * bobSpeed;
    //            }
    //            // Applies HeadBob movement
    //            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
    //        }
    //        else
    //        {
    //            // Resets when play stops moving
    //            timer = 0;
    //            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
    //        }
    //    }
}