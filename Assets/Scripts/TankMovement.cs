using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int playerNumber = 1;
    public float speed = 12f;
    public float turnSpeed = 180f;
    public AudioSource movementAudio;
    public AudioClip engineIdle;
    public AudioClip engineMovement;
    public float pitchRange = 0.2f;

    private string movementAxis;     
    private string turnAxis;         
    private Rigidbody rb;         
    private float movementInput;    
    private float turnInput;        
    private float originalPitch;         


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        rb.isKinematic = false; //Evita que el tanque sea afectado por las fisicas hasta que no le impacte una bala
        movementInput = 0f;
        turnInput = 0f;
    }


    private void OnDisable()
    {
        rb.isKinematic = true; //Hace que el tanque sea afectado por las fisicas al impactarle una bala ya que la funcion se ejecuta al impacto
    }


    private void Start()
    {
        movementAxis = "Vertical" + playerNumber;
        turnAxis = "Horizontal" + playerNumber;

        originalPitch = movementAudio.pitch;
    }
    

    private void Update()
    {
        movementInput = Input.GetAxis(movementAxis);
        turnInput = Input.GetAxis(turnAxis);

        EngineAudio();
    }


    private void EngineAudio()
    {
        if (movementInput == 0 && turnInput == 0)
        {
            if (movementAudio.clip == engineMovement)
            {
                movementAudio.clip = engineIdle;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
        else
        {
            if (movementAudio.clip == engineIdle)
            {
                movementAudio.clip = engineMovement;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        Vector3 movement = transform.forward * movementInput * speed * Time.deltaTime;

        rb.MovePosition(rb.position + movement);
    }


    private void Turn()
    {
        float turn = turnInput * turnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        rb.MoveRotation(rb.rotation * turnRotation);
    }
}