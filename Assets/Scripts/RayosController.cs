using UnityEngine;
using UnityEngine.InputSystem;

public class RayosController : MonoBehaviour
{
    [SerializeField] private Transform PivoteCamara;
    [SerializeField] private float alcanceInteracción;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private float ShootForce;

    PlayerInput Input;

    InputAction disparar;
    InputAction interactuar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Input = GetComponent<PlayerInput>();
        disparar = Input.actions["Shoot"];
        interactuar = Input.actions["Interact"];


    }

    // Update is called once per frame
    void Update()
    {
        if (disparar.WasPerformedThisFrame())
        {
            Disparar();
        }
        if (interactuar.WasPerformedThisFrame())
        {
            Interactuar();
        }

    }


    private void Disparar()
    {
        Ray rayo = new Ray(PivoteCamara.position, PivoteCamara.forward);

        Debug.DrawRay(rayo.origin, rayo.direction, Color.red, 5f);

        GameObject bullet = Instantiate(bulletPrefab, PivoteCamara.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(rayo.direction * ShootForce, ForceMode.Impulse);

        /*
        RaycastHit hit;
        if (Physics.Raycast(rayo, out hit, alcanceInteracción))
        {
            Debug.Log("Piu");
            if (hit.collider.gameObject.tag == ("Castañas"))
            {
                Destroy(hit.collider.gameObject);
            }

        }
        */
    }



    private void Interactuar()
    {
        Ray rayo = new Ray(PivoteCamara.position, PivoteCamara.forward);

        Debug.DrawRay(rayo.origin, rayo.direction, Color.green, 5f);

        RaycastHit hit;
        if (Physics.Raycast(rayo, out hit, alcanceInteracción))
        {
            if (hit.collider.gameObject.tag == ("Castañas"))
            {
                Debug.Log("T voy a matar >:(");
            }

        }

    }
}
