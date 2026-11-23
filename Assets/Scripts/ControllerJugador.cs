using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerJugador : MonoBehaviour
{

    [Header ("Refs")]
    [SerializeField] private Transform pivoteCamara;
    [SerializeField] private GameObject modelo;
    [SerializeField] private Camera camara;

    [Header("Movimiento")]
    [SerializeField] private float velocidad;
    [SerializeField] private float alturaSalto = 1.5f;
    [SerializeField] private float gravedad = -20;

    private float VelocidadVertical;
    private float inclinacion;

    private CharacterController controlador;

    private PlayerInput input;

    private InputAction mover;
    private InputAction saltar;
    private InputAction mirar;
    private InputAction apuntar;
    private InputAction cambVista;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
        controlador = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();

        mover = input.actions["Move"];
        saltar = input.actions["Jump"];
        mirar = input.actions["Look"];
        apuntar = input.actions["Point"];
        cambVista = input.actions["ChangeView"];


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        GirarCamara();
        MoverPersonaje();
        ActualilzarCamara();

    }

    void GirarCamara()
    {
        // obtener delta del raton
        Vector2 delta = mirar.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * delta.x);


        inclinacion -= delta.y;

        inclinacion = Mathf.Clamp(inclinacion, -80f, 80f);

        pivoteCamara.localRotation = Quaternion.Euler(inclinacion, 0f, 0f);

    }


    void MoverPersonaje()
    {
        //Lee el input
        Vector2 entrada = mover.ReadValue<Vector2>();

        //Calcula dirección de mov.
        Vector2 direccion = transform.right * entrada.x + transform.forward * entrada.y;


        direccion = Vector3.ClampMagnitude(direccion, 1f);

        //Calcula vector mov. fonal para utilizar con el move

        Vector3 desplazamiento = direccion * velocidad;

        VelocidadVertical += gravedad * Time.deltaTime;

        desplazamiento.y = VelocidadVertical;

        //Mover al personaje con el characterController

        controlador.Move(desplazamiento * Time.deltaTime);

        //Velocidad en Y (salto)

        if (controlador.isGrounded && saltar.WasPressedThisFrame())
        {
            VelocidadVertical = Mathf.Sqrt(alturaSalto * -2f * gravedad);
        }
       


    }

    void ActualilzarCamara()
    {

    }



}
