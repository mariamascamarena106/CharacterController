using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerJugador : MonoBehaviour
{

    [Header ("Refs")]
    [SerializeField] private Transform pivoteCamara;
    [SerializeField] private GameObject modelo;
    [SerializeField] private Camera camara;
    [SerializeField] private GameObject ojo;

    [Header("Movimiento")]
    [SerializeField] private float velocidad;
    [SerializeField] private float alturaSalto = 1.5f;
    [SerializeField] private float gravedad = -20;
    [SerializeField] private float sensibilidadMouse = 0.12f;
    [SerializeField] private float suavizado = 10f;

    [Header("Cámara")]
    [SerializeField] private Vector3 TerceraPos = new Vector3(0.65f, 0.15f, -3f);
    [SerializeField] private Vector3 ApuntadoPos = new Vector3(0.55f, 0.1f, -1.7f);


    private float VelocidadVertical;
    private float inclinacion;

    private bool primeraPers = false;

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

        if (cambVista.WasCompletedThisFrame())
        {
            primeraPers = !primeraPers;
            ojo.SetActive(!primeraPers);
        }

        GirarCamara();
        MoverPersonaje();
        ActualilzarCamara();

    }

    void GirarCamara()
    {
        // obtener delta del raton
        Vector2 delta = mirar.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * delta.x * sensibilidadMouse);


        inclinacion -= (delta.y * sensibilidadMouse);

        inclinacion = Mathf.Clamp(inclinacion, -80f, 80f);

        pivoteCamara.localRotation = Quaternion.Euler(inclinacion, 0f, 0f);

    }


    void MoverPersonaje()
    {
        //Lee el input
        Vector2 entrada = mover.ReadValue<Vector2>();

        //Calcula dirección de mov.
        Vector3 direccion = transform.right * entrada.x + transform.forward * entrada.y;


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

        Vector3 destino = Vector3.zero;

        if (!primeraPers)
        {
            destino = TerceraPos;
        }
       

        camara.transform.localPosition = Vector3.Lerp(
            camara.transform.localPosition,
            destino,
            suavizado * Time.deltaTime);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Castañas")
        {
            Destroy(hit.gameObject);
        }
    }


}
