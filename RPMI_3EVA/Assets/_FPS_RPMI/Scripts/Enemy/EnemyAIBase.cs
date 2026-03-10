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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
