using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam;// ref si disparamos desde el centro de la cam
    [SerializeField] Transform shootPoint;// ref si queremos disparar desde la punta del cañom
    [SerializeField] LayerMask impactLayer;// layer con la que el raycast interactua
    RaycastHit hit; // Almacen de la información de los objetos a los que el Raycast puede impactar

    [Header("Weapon Parameters")]
    [SerializeField] int damage = 10;// Daño del arma por bala
    [SerializeField] float range = 100f;// Distancia de disparo
    [SerializeField] float spread = 0;// Radio de dispersión del arma
    [SerializeField] float shootingCooldown = 0.2f;// tempo entre disparos
    [SerializeField] float reloadTime = 1.5f;// tempo de recarga en segundos
    [SerializeField] bool allowButtomHold = false;//si el disparo se ejecuta por click(falso) o por mantener(true)

    [Header("Bullet Management")]
    [SerializeField] int ammoSize = 30;// Cantidad max de balas/cargador
    [SerializeField] int bulletsPerTap = 1;// Cantidad de balas disparadas por cada ejecución de disparo
    int bulletsLeft;// cantidad de balas dentro del cargador actual

    [Header("Feedback References")]
    [SerializeField] GameObject impactEffect;// ref al vfx de impacto bala

    [Header("Dave-Gun State Bools")]
    [SerializeField] bool shoting;// indica si estamos disparando
    [SerializeField] bool canShoot;// indica si podemos disparar en x
    [SerializeField] bool reloading;
    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize;// al iniciar la partida tenemos el cargador lleno
        canShoot = true;// al iniciar la partida,tenemos la posibilidad de disparar
    }

  

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        // este es el metodo mas importante
        // aqui se define el disparo por raycast= utilizable con cualquier mecanica

        // almacenar la dirreción de disparo y modificarla en caso de haber spread
        Vector3 direction= fpsCam.transform.forward;// se lanza rayo hacia delante de la camara
        // añadir dispersion aleatoria segun el valor de spread
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);
        // DECLARACIÓN DEL RAYCAST
        // physics.Raycast(origen del rayo,dirección,almacen de la info del impacto, longitud del rayo, layer con la que impacta el rayo)
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, impactLayer))
        {
            //AQUI PUEDO CODEAR TODOS LOS EFECTOS QUE QUIERO PARA MI INTERACCIÓN
            Debug.Log(hit.collider.name);
        }
    }


    #region Input Metods

    public void OnShoot(InputAction.CallbackContext context)
    {

    }
    public void OnReload(InputAction.CallbackContext context)
    {

    }
    #endregion
}
