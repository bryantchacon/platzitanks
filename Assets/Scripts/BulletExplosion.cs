using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    public LayerMask tankLayer;
    public ParticleSystem explosionParticles;
    public AudioSource explosionAudio;
    public float maxDamage = 100f;
    public float explosionForce = 1000f;
    public float maxLifeTime = 2f;
    public float explosionRadius = 5f;


    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        //Guarda en un array el/los tanques que alcance el radio de la explosion
        Collider[] affectedTanks = Physics.OverlapSphere(transform.position, explosionRadius, tankLayer);

        for (int i = 0; i < affectedTanks.Length; i++)
        {
            Rigidbody targetRigidBody = affectedTanks[i].GetComponent<Rigidbody>();

            if (!targetRigidBody)
            {
                continue;
            }

            targetRigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            TankHealth targetHealth = targetRigidBody.GetComponent<TankHealth>();

            if (!targetHealth)
            {
                continue;
            }

            float damage = CalculateDamage(targetRigidBody.position);

            targetHealth.TakeDamage(damage);
        }

        //Deshace el parentesco de la explosion con la bala
        explosionParticles.transform.parent = null;

        explosionParticles.Play();
        explosionAudio.Play();

        Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        //Cacula la distancia del vector entre el target y la explosion
        Vector3 explosionToTarget = targetPosition - transform.position;

        //Cacula el largo del vector entre el target y la explosion
        float explosionDistance = explosionToTarget.magnitude;

        //Calcula un valor de 0 a 1 de la distancia de la explosion
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        //Calcula el % de la explosion que alcanza al target
        float damage = relativeDistance * maxDamage;

        //Esto es para evitar que el daño sea negativo, si lo es se setea a 0, o sea la explosion no alcanzo al target
        damage = Mathf.Max(0f, damage);

        return damage;
    }
}