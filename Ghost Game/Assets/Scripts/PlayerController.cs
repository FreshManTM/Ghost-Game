using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public event System.Action OnReachedEndOfLevel;
    [Header("Variables")]
    [SerializeField] float speed = 6f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundDistance = 0.4f;

    [Header("Filled field")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool isGrounded;

    
    float turnSoothVelocity;
    Vector3 moveDirection;

    bool disabled;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        EnemyAI.OnEnemyHasSpottedPlayer += Disable;
    }
    void Update()
    {
        Movement();
        DoorTrig();
    }

    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = Vector3.zero;
        if (!disabled && !FindObjectOfType<GameManager>().isDialog) 
        { 
            direction = new Vector3(horizontal, 0f, vertical).normalized;
        }


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 temp = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            moveDirection.x = temp.x * speed;
            moveDirection.z = temp.z * speed;
            moveDirection.y += gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            moveDirection.x = 0;
            moveDirection.z = 0;
            moveDirection.y += gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -2f;
        }
    }

    private void Disable()
    {
        disabled = true;
    }
    void OnDestroy()
    {
        EnemyAI.OnEnemyHasSpottedPlayer -= Disable;
    }  
    void DoorTrig()
    {
        if (FindObjectOfType<GameManager>().hasKey == true && FindObjectOfType<GameManager>().inRangeOfDoor == true)
        {
            Disable();
            if (OnReachedEndOfLevel != null)
            {
                OnReachedEndOfLevel();
            }
        }
    }
}
    

