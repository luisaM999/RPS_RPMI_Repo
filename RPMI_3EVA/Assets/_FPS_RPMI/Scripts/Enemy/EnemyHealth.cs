using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [Header("Health System Configuration")]
    [SerializeField] int health;// Vida actual del enemigo
    [SerializeField] int maxhealth;//Vida maxima del enemigo

    [Header("Feedback Configuration")]
    [SerializeField] Material damagedMat;// ref al material que da feedback de dañado
    [SerializeField] MeshRenderer enemyRend;// ref al renderer del modelo del enemigo
    [SerializeField] GameObject deathVfx;//ref al sistema de particulas
    Material baseMat;// ref al material base del modelo del enemigo


    private void Awake()
    {
        health = maxhealth;//
        baseMat = enemyRend.material;// se almacena el material base del modelo del enemigo
    }
    // Update is called once per frame
    void Update()
        
    {
        if(health <= 0)
        {
            health = 0;//la vida no puede bajar de cero
            deathVfx.SetActive(true);// encendemos el vfx de muerte
            deathVfx.transform.position=transform.position;// ponemos el vfx en la posicion actual del enemigo

            gameObject.SetActive(false);//se apaga el enemigo="muerte"
        }
    }
    public void TakeDamage(int damage)
    {
        health-= damage;// quitar tanta vida como valor de daño viene de afuera
        enemyRend.material=damagedMat;// se cambia temporalmente el material base por el  material dañado
        Invoke(nameof(ResetEnemyMat), 0.1f);
    }
     void ResetEnemyMat()
    {
        enemyRend.material=baseMat;
    }
  
}
