using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.WebCam;

public class FPS_Controller : MonoBehaviour
{
    #region General Variables
    [Header("Movement & Look")]
    [SerializeField] GameObject camHolder;// ref al objeto que tiene como hijo la camara (rota por la camara)
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float maxforce = 1f;// fuerza maxima de aceleración
    [SerializeField] float sensitivity = 0.1f;// Sensibilidad para el imput

    [Header("Jump and Groundcheck")]
    [SerializeField] float jumpforce = 5f;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundcheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;
    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    #endregion


    //Variable de referencias privadas
    Rigidbody rb; // ref al rigidbody del player

    // Variable para el input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // lock del cursor del raton
        Cursor.lockState = CursorLockMode.Locked; //Mueve el cursor al centro
        Cursor.visible = false;// Oculta el cursor de la vista
    }

    // Update is called once per frame
    void Update()
    {
        // Groundcheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundcheckRadius, groundLayer);
        //Dibujar un rayo ficticio en escena para determinar la orientación de la cámara
        Debug.DrawRay(camHolder.transform.position, camHolder.transform.forward * 180f, Color.red);

    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void LateUpdate()
    {
        CameraLook();
    }

    void CameraLook()
    {
        //Rotacion horizontal del cuerpo del personaje
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        //Rotacion vertical(la lleva la camara)
        lookRotation += (-lookInput.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.localEulerAngles = new Vector3(lookRotation, 0f, 0f);
    }
    void Movement()
    {
        Vector3 currentVelocity = rb.linearVelocity; // Necesitamos calcular la velocidad actual del rb constantemente
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y);// Velocidad a alcanzar= la dirección que pulsamos
        targetVelocity *= isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed);
        //Convertir la dirección local en global
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calcular el cambio de Velocidad (aceleración)
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxforce);

        //Aplicar la fuerza de movimiento/aceleración
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Jump()
    {
        if(isGrounded) rb.AddForce(Vector3.up* jumpforce,ForceMode.Impulse);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput=context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
            //Añadir la animación de agacharse
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput= context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.performed && !isCrouching) isSprinting = true;
        if (context.canceled) isSprinting=false;

    }

    #region Input Methods
    #endregion
}
