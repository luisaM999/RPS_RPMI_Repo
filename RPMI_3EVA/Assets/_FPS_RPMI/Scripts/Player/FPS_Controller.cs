using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller : MonoBehaviour
{
    #region General Variables
    [Header("Movement & Look")]
    [SerializeField] GameObject CamHolder;// ref al objeto que tiene como hijo la camara (rota por la camara)
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float maxforce = 1f;// fuerza maxima de aceleración
    [SerializeField] float sensitivity = 0.1f;// Sensibilidad para el imput

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
        rb=GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // lock del cursor del raton
        Cursor.lockState= CursorLockMode.Locked; //Mueve el cursor al centro
        Cursor.visible= false;// Oculta el cursor de la vista
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput= context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {

    }
    public void OnCrouch(InputAction.CallbackContext context)
    {

    }
    public void OnSprint(InputAction.CallbackContext context)
    {

    }
    #endregion
}
