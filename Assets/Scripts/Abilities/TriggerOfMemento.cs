using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOfMemento : MonoBehaviour
{
    private MementoEntity _playerDataMementoEntity;
    public static bool ItsRewindTime { get; private set; }

    //public PCMouseLook mouseLookScript;
    //public PCPlayerMovement PCplayerMovementScript;
    //public PlayerInputs PlayerInputsScript;
    //public FirstPersonController FPCScript;

    public PlayerMovementGrappling grapScript;
    public Grappling anotherGrapScript;
    public Climbing ClimbScript;
    public LedgeGrabbing grabbingScript;
    public WallRunningAdvanced WallRunningAdvancedScript;
    public Sliding slidingScript;
    public PlayerCam PlayerCamScript;
    public GrapplingRope_MLab GrapplingRope_MLabScript;
    //public Rigidbody RigidbodyScript;
    public Collider Collider;
    public static bool ExecuteCoroutine { get; private set; }

    private float _timer;

    private Coroutine _loadMemoriesCoroutine;

   // [SerializeField] private AudioClip RewindTimeSound;
    //[SerializeField] private AudioSource ThisAudioSource;
    void Start()
    {
        //Guardamos una lista de todas las entidades que hay en la escena
        // _playerDataMementoEntity = new List<MementoEntity>(FindObjectsOfType<MementoEntity>());
        _playerDataMementoEntity = FindObjectOfType<MementoEntity>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExecuteMementoFunc();

        }
        MementoMechanicActivated();
    }
    public void MementoMechanicActivated()
    {
        if (ItsRewindTime == true)
        {
            if (ExecuteCoroutine == false)
            {
                //mouseLookScript.enabled = false;
                //PCplayerMovementScript.enabled = false; PC
                //FPCScript.enabled = false;
                // PlayerInputsScript.enabled = false;
                grapScript.enabled = false;
                anotherGrapScript.enabled = false;
                ClimbScript.enabled = false;
                grabbingScript.enabled = false;
                WallRunningAdvancedScript.enabled = false;
                slidingScript.enabled = false;
                PlayerCamScript.enabled = false;
                GrapplingRope_MLabScript.enabled = false;
                //Collider.enabled = false;
                
                _loadMemoriesCoroutine = StartCoroutine(LoadAllMemories());
                ExecuteCoroutine = true;
            }
            _timer += Time.deltaTime;
            if (_timer >= 5f)
            {
                //Collider.enabled = true;
                ItsRewindTime = false;
                StopCoroutine(_loadMemoriesCoroutine);
                //FPCScript.enabled = true;
                //mouseLookScript.enabled = true;
                // PCplayerMovementScript.enabled = true; PC
                //PlayerInputsScript.enabled = true;
                grapScript.enabled = true;
                anotherGrapScript.enabled = true;
                ClimbScript.enabled = true;
                grabbingScript.enabled = true;
                WallRunningAdvancedScript.enabled = true;
                slidingScript.enabled = true;
                PlayerCamScript.enabled = true;
                GrapplingRope_MLabScript.enabled = true;


                _timer = 0;
                ExecuteCoroutine = false;
            }


        }
    }
    IEnumerator LoadAllMemories()
    {
        while (true)
        {

            _playerDataMementoEntity.TryLoadStates();
            yield return null;
        }
    }
    public void PlayAudio(AudioClip AC)
    {
        //ThisAudioSource.clip = AC;

        //ThisAudioSource.Play();

    }
    public void ExecuteMementoFunc()
    {
        ItsRewindTime = true;
        Debug.Log("se ejecuta el memento");
        //PlayAudio(RewindTimeSound);
    }
    
/*
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 24)
        {
            other.gameObject.SetActive(false);
            Destroy(other);
            ItsRewindTime = true;
            Debug.Log("se ejecuta el memento");
            PlayAudio(RewindTimeSound);
        }
    }*/
}
