using System.Collections;
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
    [SerializeField] bool shooting;// indica si estamos disparando
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
        //condición estricta de llamar a la rutina de disparo
        if (canShoot && shooting && !reloading && bulletsLeft > 0)
        {
            StartCoroutine(ShootRoutine());
        }
    }
    IEnumerator ShootRoutine()
    {
        //la corrutina se va a encargar de medir el tiempo entre disparos y la gestion del gesto de balas
        // además llamará al raycast de disparo que esta definido en Shoot()
        canShoot = false;// llave de seguridad que hace que si estamos disparando no podamos disparar
        if (!allowButtomHold) shooting = false;// cerrar el bucle de disparo por pulsación
        for (int i = 0;i<bulletsPerTap; i++)
        {
            if(bulletsLeft<=0)break;//Segunda prevención de errores: si no me quedan balas no hago daño
            Shoot();//llamada al raycast que define el disparo
            bulletsLeft--;// resta 1 a la cantidad de balas del cargador actual
        }
        //Espera entre disparos
        yield return new WaitForSeconds(shootingCooldown);
        canShoot=true;// Resetea la posibilidad de disparar
    }

    void Shoot()
    {
        // este es el metodo mas importante
        // aqui se define el disparo por raycast= utilizable con cualquier mecanica

        // almacenar la dirreción de disparo y modificarla en caso de haber spread
        Vector3 direction = fpsCam.transform.forward;// se lanza rayo hacia delante de la camara
        // añadir dispersion aleatoria segun el valor de spread
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);
        // DECLARACIÓN DEL RAYCAST
        // physics.Raycast(origen del rayo,dirección,almacen de la info del impacto, longitud del rayo, layer con la que impacta el rayo)
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, impactLayer))
        {
            //AQUI PUEDO CODEAR TODOS LOS EFECTOS QUE QUIERO PARA MI INTERACCIÓN
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth= hit.collider.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(damage);
            }
        }
    }
    void Reload()
    {

        if (bulletsLeft < ammoSize && !reloading) StartCoroutine(ReloadRoutine());
    }
    IEnumerator ReloadRoutine()
    {
        reloading = true; //Estamos recargando , por lo tanto no podemos recargar
        //AQUI LLAMARIAMOS A LA ANIMACIÓN DE RECARGA
        yield return new WaitForSeconds(reloadTime);//Esperar tanto tiempo como dura la animacion de recarga
        bulletsLeft = ammoSize;// la cantidad de balas actuales se iguala a la máxima
        reloading = false;//termina la recarga, podemos volver a recargar
    }



    #region Input Metods

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (allowButtomHold)
        {
            shooting = context.ReadValueAsButton();//Detecta constantemente si el botón de disparo está apretado
        }
        else
        {
            if (context.performed) shooting = true;
        }

    }
            public void OnReload(InputAction.CallbackContext context)
            {
                if (context.performed) Reload();
            }
    #endregion
}
