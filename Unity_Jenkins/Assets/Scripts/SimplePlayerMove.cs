using UnityEngine;

public class SimplePlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    void Update()
    {
        // 1. Input lena
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // 2. Movement Logic
        bool isMoving = move.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = 0;

        if (isMoving)
        {
            if (isRunning)
            {
                currentSpeed = runSpeed;
                anim.SetInteger("State", 2); // Run
            }
            else
            {
                currentSpeed = walkSpeed;
                anim.SetInteger("State", 1); // Walk
            }
        }
        else
        {
            currentSpeed = 0;
            anim.SetInteger("State", 0); // Idle
        }

        // 3. Move Character
        controller.Move(move * currentSpeed * Time.deltaTime);
    }
}