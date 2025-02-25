using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraTransitionsEvents : MonoBehaviour
{
    public GameObject[] NewTraps;  // Array de NewTraps que la c�mara seguir�
    public GameObject[] BrandNewTraps;
    public GameObject[] PackOfTraps;
    public GameObject[] Level2Traps;
    public GameObject[] Level3Traps;
    private int CounterForMissingTraps;
    public int CurrentTrap = 0;
    public int CurrentBrandNewTrap = 0;
    public bool TransitionOn;

    public Animator Transitionanimator;  // Asigna esto desde el Inspector
    public string animationName; // Nombre de la animaci�n o trigger
    public GameObject Player;
    public GameObject CanvasHud;
    public GameObject[] CollisionTrigger;
    public GameObject EndLevel;
    public MoveCamera moveCameraScript;
    public Camera mainCamera;
    public Camera transitionCamera;

    public GameObject Tapa; // La tapa a animar
    public float velocidadTapa = 10f; // Velocidad ajustable de la tapa

    //
    [SerializeField] private PlayerMovementGrappling playerMovement;
    [SerializeField] private PlayerCam camMovement;
    //[SerializeField] private Grappling grappling;
    //
    private bool AnimLVL1Triggered;
    public int currentSceneIndex;
    //MUY IMPORTENTE: SI NO ESTAS EN EL NIVEL 1 TILDAR EL BOLL A TRUE
    private void OnEnable()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
   
    public void SkipCinematic()
    {
        
        for(int i = 0; i < NewTraps.Length; i++)
        {
            if (NewTraps[i] != null)
                NewTraps[i].SetActive(true);
        }
        for (int i = 0; i < BrandNewTraps.Length; i++)
        {
            if(BrandNewTraps[i] != null)
                BrandNewTraps[i].SetActive(true);
        }
        for (int i = 0; i < PackOfTraps.Length; i++)
        {
            if(PackOfTraps[i]!=null)
                PackOfTraps[i].SetActive(true);
        }
        for (int i = 0; i < Level2Traps.Length; i++)
        {
            if(Level2Traps[i]!=null)
                Level2Traps[i].SetActive(true);
        }
        for (int i = 0; i < Level3Traps.Length; i++)
        {
            if(Level3Traps[i]!=null)
                Level3Traps[i].SetActive(true);
        }
        if (AnimLVL1Triggered == false && currentSceneIndex==8)
        {
            MultipleLaserAnimation(NewTraps[20], NewTraps[19]);
        }
        EndCinematicFunc();

    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SkipCinematic();
        }
    }
    */

    public void Level2TurnOnLaser()
    {
        Level2Traps[CurrentBrandNewTrap].gameObject.SetActive(true);
        CurrentBrandNewTrap++;
    }

    private IEnumerator MultipleLaserAnimation(GameObject pieza, GameObject pieza2)
    {
        // Animaci�n de la tapa bajando -20 en Y y luego -20 en Z
        Vector3 destinoTapa1 = Tapa.transform.position + new Vector3(0, -1f, 0);
        Vector3 destinoTapa2 = destinoTapa1 + new Vector3(0, 0, -5f);

        while (Vector3.Distance(Tapa.transform.position, destinoTapa1) > 0.01f)
        {
            Tapa.transform.position = Vector3.MoveTowards(Tapa.transform.position, destinoTapa1, velocidadTapa * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(Tapa.transform.position, destinoTapa2) > 0.01f)
        {
            Tapa.transform.position = Vector3.MoveTowards(Tapa.transform.position, destinoTapa2, velocidadTapa * Time.deltaTime);
            yield return null;
        }
        

        Vector3 destino = pieza.transform.position + new Vector3(0, 2.7849f, 0);
        Vector3 destino2 = pieza2.transform.position + new Vector3(0, 2.7849f, 0);

        float duracion = 1.5f; // Duraci�n de la animaci�n
        float velocidad = 7f / duracion; // Velocidad constante

        while (Vector3.Distance(pieza.transform.position, destino) > 0.01f&& Vector3.Distance(pieza2.transform.position, destino2) > 0.01f)
        {
            pieza.transform.position = Vector3.MoveTowards(pieza.transform.position, destino, velocidad * Time.deltaTime);
            pieza2.transform.position = Vector3.MoveTowards(pieza2.transform.position, destino2, velocidad * Time.deltaTime);
            yield return null;

        }

        NewTraps[21].gameObject.SetActive(true);
        NewTraps[22].gameObject.SetActive(true);
        NewTraps[23].gameObject.SetActive(true);
        NewTraps[24].gameObject.SetActive(true);
        pieza.transform.position = destino;
        pieza2.transform.position = destino2;
        CurrentTrap = 25;
        AnimLVL1Triggered = true;

    }
    public void FixingThirdLevelMissingLasers()
    {
        Level3Traps[CounterForMissingTraps].gameObject.SetActive(true);
        CounterForMissingTraps++;
    }
    public void ThirdLevel()
    {
        NewTraps[CurrentTrap].gameObject.SetActive(true);
        CurrentTrap++;
    }

    public void SecondTransition()
    {
        BrandNewTraps[CurrentBrandNewTrap].gameObject.SetActive(true);
        CurrentBrandNewTrap++;
    }
    public void TurnOnLasersPack()
    {
        for (int i = 0; i < 7; i++)
        {
            PackOfTraps[i].gameObject.SetActive(true);
            
        }
    }

    public void TransitionFunction()
    {
        if (CurrentTrap == 18)
        {
            StartCoroutine(MultipleLaserAnimation(NewTraps[20], NewTraps[19]));
            Debug.Log("animacion");

        }
        else if (CurrentTrap==8)
        {
            for (int i = 8;i <= 17; i++)
            {
                NewTraps[CurrentTrap].gameObject.SetActive(true);
                CurrentTrap++;
            }
        }
        else
        {
            NewTraps[CurrentTrap].gameObject.SetActive(true);
            CurrentTrap++;
            Debug.Log("xd");

        }
    }

    public void EndCinematicFunc()
    {
        Transitionanimator.enabled = false;
        transitionCamera.enabled = false;
        transitionCamera.gameObject.SetActive(false);

        moveCameraScript.enabled = true;
        //Player.gameObject.SetActive(true);

        //
        playerMovement.enabled = true;
        camMovement.enabled = true;
        //grappling.enabled = true;
        //

        CanvasHud.gameObject.SetActive(true);
        for (int i = 0; i < CollisionTrigger.Length; i++)
        {
            CollisionTrigger[i].gameObject.SetActive(true);

        }
        EndLevel.SetActive(true);
        mainCamera.enabled = true;


    }
}
