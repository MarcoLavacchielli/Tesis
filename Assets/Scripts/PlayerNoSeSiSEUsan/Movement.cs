using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;

    Vector3 velocity;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    [SerializeField] private AudioClip WalkSound;
    [SerializeField] private AudioSource ThisAudioSource;
    bool _isWalking;

    //private Imovement walk;
    //private Imovement run;

    /*[SerializeField] private Transform Upfloor;
    [SerializeField] private Transform ElevatorTrigger;*/
    // Update is called once per frame

    private void Awake()
    {

        //walk = new PCWalk(transform, 6f, controller, velocity, gravity, groundCheck, groundDistance, groundMask, isGrounded, WalkSound, ThisAudioSource, _isWalking);

        //run = new PCRun(transform, 12f, controller, velocity, gravity, groundCheck, groundDistance, groundMask, isGrounded, WalkSound, ThisAudioSource, _isWalking);


        var InputsScript = GetComponent<PlayerInputs>();

        if (!InputsScript) return;

        InputsScript.MovementFuncInputs += MovementFunction;
        //InputsScript.StrategyWalkFuncInputs += StrategyWalk;
        //InputsScript.StrategyRunFuncInputs += StrategyRun;

    }
    /*public void FixedUpdate()
    {
        if (Input.GetButton("Jump"))
        {
            StrategyRun();
        }
        else
        {
            StrategyWalk();
        }
        
       
    }*/
    /*
    public void StrategyWalk()
    {
        walk.movement();

    }
    public void StrategyRun()
    {
        run.movement();

    }
    */
    public void PlayAudio(AudioClip AC)
    {
        ThisAudioSource.clip = AC;

        ThisAudioSource.Play();

    }

    public void MovementFunction()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (move != Vector3.zero)
        {
            if (_isWalking == false)
            {
                PlayAudio(WalkSound);
                _isWalking = true;
            }
        }
        else
        {
            ThisAudioSource.Stop();
            _isWalking = false;
        }
        controller.Move(move * speed * Time.deltaTime);
        /* if (controller.Move(move * speed * Time.deltaTime) == 0)
         {
             ThisAudioSource.Stop();
         }
         else
         {
             PlayAudio(WalkSound);
         }*/
        velocity.y += gravity + Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 15)
        {
            this.transform.position = Upfloor.position;
            print("hola");
        }
    }*/
    /*private void Teleport()
     {
         Vector3 distance = transform.position - ElevatorTrigger.position;
         float dis=distance.magnitude;
         print(dis);
         if (dis < 1.0f)
         {
             transform.position = Upfloor.position;
             print("estofunciona");
         }
         {
             print("nofunciona");
         }
     }*/


}
