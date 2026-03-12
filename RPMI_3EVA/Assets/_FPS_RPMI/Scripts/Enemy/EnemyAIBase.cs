using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAIBase : MonoBehaviour
{
    #region General Variables
    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent;// ref al cerebro
    [SerializeField] Transform target;//
    [SerializeField] LayerMask targetLayer;// define layer del target
    [SerializeField] LayerMask groundLayer;// define layer del suelo( evita ir a zonas sin suelo)
    [Header("Patrolling Stats")]
    [SerializeField] float walkPointRange = 10f;//radio maximo para determinar puntos a perseguir
    Vector3 walkPoint;// posicion del punto random a perseguir
    bool walkPointSet;// hay punto a perseguir generado? si es false, genera uno
    [Header("Patrolling Stats")]
    [SerializeField] float timeBetweenAttacks = 1f;//Col
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootSpeedY; // Fuerza de disparo hacia arriba
    [SerializeField] float shootSpeedZ= 10f; // Fuerza de disparo hacia DELANTE
    bool alreadyAttacked; // si es verdadero no stackea ataques y entra
    [Header("State & Detection")]
    [SerializeField] float sightRange = 8f;// radio del detector de persecucion
    [SerializeField] float attackRange = 2f;// radio del detector de ataque
    [SerializeField] bool targetInSightRange;// determina si es verdadero que podemos perseguir al target
    [SerializeField] bool targetInAttackRange;// determina si es verdadero que podemos atacar al target
    [Header("Stuck Detection")]
    [SerializeField] float stuckCheckTime = 2f;// tiempo que la gente espera estando quieto antes de darse cuenta
    [SerializeField] float stuckThreshold = 0.1f;// margen de detección de stuck
    [SerializeField] float maxstuckThreshold = 3f;// tiempo maximo de estar stuck

    float stuckTimer;// Reloj que cuenta el tiempo de estar stuck
    float lastCheckTime;// tiempo de chequeo previo de stuck
    Vector3 lastPosition;// Posición del ultimo walkpoint perseguir
    #endregion
    private void Awake()
    {
        target= GameObject.Find("Player").transform;
        agent = GetComponent< NavMeshAgent >();
        lastPosition= transform.position;
        lastCheckTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyStateUpdate();
    }
    void EnemyStateUpdate()
    {
        // metodo que se encarga de gestionar el cambio de estados del enemigo
        //1_Cambio de estado de los bools
        // primero detectamos si los targets estan en vision
        Collider[]hits= Physics.OverlapSphere(transform.position,sightRange,targetLayer);
        targetInSightRange = hits.Length > 0;
        //
        if(targetInSightRange)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            targetInAttackRange = distance <= attackRange;
        }
        else
        {
            targetInAttackRange = false;
        }


        if (!targetInSightRange && !targetInAttackRange)
        {
            Patroling();
        }
        else if (targetInSightRange && !targetInAttackRange)
        {
            ChaseTarget();
        }
        else if (targetInSightRange && targetInAttackRange)
        {
            AttackTarget();
        }
    }


    void Patroling()
    {
        Debug.Log("Enemigo en estado patrulla");
    }
    void ChaseTarget()
    {
        //acción que le dice a la gente que persiga al target
        agent.SetDestination(target.position);
    }
    void AttackTarget()
    {
        //acción que contiene la logica de ataque
        //1- hacer q la gente se quede quieto(perseguirse a si mismo)
        agent.SetDestination(transform.position);
        //2- Aplicar una rotación suavizada para que el agente mire al target antes de atacar
        Vector3 direction=(target.position - transform.position).normalized;
        if(direction == Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, agent.angularSpeed* Time.deltaTime);
        }

        //3- se ataca (solo si no se esta atacando)
        if(!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack),timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked=false;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return; // si estamos jugando en build no se ejecuta el resto del codigo

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
