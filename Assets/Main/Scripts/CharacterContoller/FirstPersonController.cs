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
    [SerializeField]private float gravityScale = 1f;
    private bool bowGrabbed = false;


    #region Movement Variables
    private Rigidbody rb;
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    #region Sprint
    public KeyCode sprintKey = KeyCode.LeftShift;
    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public float sprintSpeed = 7f;
    #endregion

    #region UI
    public KeyCode pauseButton = KeyCode.Joystick1Button7;
    private Camera playerCamera;
    public TextMeshProUGUI screenInfoText;
    public Animator scrennInfoAnim;
    #endregion

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Joystick1Button0;
    public float jumpPower = 5f;
    public bool isGrounded = false;
    // Internal Variables

    private IEnumerator powerUp;
    private bool speedMofiied = false;
    private bool jumpModified = false;
    #endregion

    #endregion

    #region ExternalScripts
    private MailBoxContoller mbController;
    public PowerUpController puController;
    #endregion


    public UIManager uImanager;
    private bool gameIsPaused = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        mbController = GameObject.FindGameObjectWithTag("MailBoxController").GetComponent<MailBoxContoller>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            if (enableJump && !gameIsPaused)
            {
                CheckGround();
                if (isGrounded)
                {
                    Jump();
                    Debug.Log("Jump");
                }

            }
        }
        if (Input.GetKeyUp(pauseButton))
        {
            if (!gameIsPaused)
            {
                gameIsPaused = true;
            }
            else
            {
                gameIsPaused = false;
            }
            uImanager.ToggleOptionsMenu();
            Debug.Log("Paused");
        }

    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravityScale);
        if (playerCanMove && bowGrabbed)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (targetVelocity.x == 0 && targetVelocity.z == 0)
            {
                CheckGround();
            }
            else
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);

                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }
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
    }
    private void Jump()
    {
        rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
        isGrounded = false;
    }
    
    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            ScreenInfoActivate("Speed Boost! 10 Seconds");

            AudioSource source = GameObject.FindGameObjectWithTag("PowerUp").GetComponent<AudioSource>();
            source.Play();

            if (powerUp != null)
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
            puController.GenerateRandomSpawnLocation();

            Debug.Log("Speedboost");

        }
        if (other.gameObject.CompareTag("JumpBoost"))
        {
            ScreenInfoActivate("Jump Boost! 10 Seconds");

            AudioSource source = GameObject.FindGameObjectWithTag("PowerUp").GetComponent<AudioSource>();
            source.Play();

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
            puController.GenerateRandomSpawnLocation();

            Debug.Log("JumpBoost");
        }
        if (other.gameObject.CompareTag("TimeAdd"))
        {
            ScreenInfoActivate("+5 Seconds!");

            AudioSource source = GameObject.FindGameObjectWithTag("PowerUp").GetComponent<AudioSource>();
            source.Play();

            Destroy(other.gameObject);
            puController.GenerateRandomSpawnLocation();

            mbController.TimeResetPowerUp();

            Debug.Log("TimeAdd");
        }
        if (other.gameObject.CompareTag("LaunchPad"))
        {
            ScreenInfoActivate("Boing!");

            AudioSource source = GameObject.FindGameObjectWithTag("JumpPad").GetComponent<AudioSource>();
            source.Play();
            
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * 5000f);

            Debug.Log("LaunchPad");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("LaunchPad"))
        {
            rb.AddForce(Vector3.up * 100f);
        }
    }
    #endregion
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
    public void ScreenInfoActivate(string infoText)
    {
        scrennInfoAnim.gameObject.SetActive(false);
        scrennInfoAnim.gameObject.SetActive(true);

        screenInfoText.text = infoText;
        scrennInfoAnim.SetTrigger("ScreenInfoTrigger");
    }

    public void BowHasBeenGrabbed()
    {
        bowGrabbed = true;
    }
    
}