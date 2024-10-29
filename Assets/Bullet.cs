using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3f; // Velocidad inicial del proyectil
    Vector3 direction; // Dirección del proyectil
    private float lifetime = 5f; // Tiempo de vida máximo de la bala en segundos

    // Establecer la dirección del proyectil
    public void setDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // Mover el proyectil en la dirección establecida
        transform.position += direction * speed * Time.deltaTime;
        speed += 10f * Time.deltaTime; // Aumentar gradualmente la velocidad

        // Resta el tiempo de vida y destruye la bala si ha alcanzado su límite
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Detectar colisiones con enemigos
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Destruir al enemigo
            Destroy(other.gameObject);
            // Destruir la bala
            Destroy(gameObject);
            // Incrementar el contador de enemigos eliminados
            PlayerController.instance.IncreaseEnemyCount();
        }
    }
}
