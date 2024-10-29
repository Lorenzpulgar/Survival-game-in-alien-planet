using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para manipulación de elementos UI como texto

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance; // Instancia estática para acceder al controlador desde Bullet

    // Referencias a objetos en la escena
    [SerializeField] private GameObject bullet; // Prefab de la bala que se dispara
    [SerializeField] private Transform rifleStart; // Posición inicial del disparo de la bala
    [SerializeField] private Text HpText; // Texto de UI para mostrar la salud
    [SerializeField] private GameObject GameOver; // UI de Game Over
    [SerializeField] private GameObject Victory; // UI de Victoria

    // Variables de salud y movimiento del jugador
    public float health = 100;
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento del astronauta
    [SerializeField] private float jumpForce = 7f; // Fuerza del salto del astronauta
    private bool isGrounded = true; // Verifica si el personaje está en el suelo
    private Rigidbody rb; // Referencia al componente Rigidbody

    // Contador de enemigos y total de enemigos para ganar el juego
    private int enemiesKilled = 0; // Contador de enemigos eliminados
    private int totalEnemies = 3; // Cambia este valor al total de enemigos en la escena

    void Awake()
    {
        // Asigna esta instancia a la variable estática para que pueda ser accedida desde Bullet
        instance = this;
    }

    // Método Start se llama al inicio del juego
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Asigna el componente Rigidbody
        UpdateHealthUI(); // Actualiza la salud al comenzar el juego
    }

    // Método Update se llama cada frame
    void Update()
    {
        HandleMovement(); // Controla el movimiento del personaje
        HandleShooting(); // Controla el disparo de balas
        HandleNearbyActions(); // Controla acciones con objetos cercanos
    }

    // Método para cambiar la salud del jugador
    public void ChangeHealth(int hp)
    {
        health += hp; // Incrementa o reduce la salud
        health = Mathf.Clamp(health, 0, 100); // Ajusta la salud entre 0 y 100
        UpdateHealthUI(); // Actualiza el texto de la salud en UI

        if (health <= 0)
            Lost(); // Llama al método Lost si la salud es 0 o menos
    }

    // Método para actualizar la interfaz de usuario de salud
    private void UpdateHealthUI()
    {
        HpText.text = health.ToString(); // Muestra la salud en el texto de UI
    }

    // Método para activar la pantalla de victoria
    public void Win()
    {
        Victory.SetActive(true); // Activa la UI de victoria
        Destroy(GetComponent<PlayerLook>()); // Desactiva el control de vista del jugador
        Cursor.lockState = CursorLockMode.None; // Libera el cursor
    }

    // Método para activar la pantalla de derrota
    public void Lost()
    {
        GameOver.SetActive(true); // Activa la UI de Game Over
        Destroy(GetComponent<PlayerLook>()); // Desactiva el control de vista del jugador
        Cursor.lockState = CursorLockMode.None; // Libera el cursor
    }

    // Método para manejar el movimiento y el salto del astronauta
    private void HandleMovement()
    {
        // Control de movimiento en el eje horizontal y vertical
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Crea un vector para el movimiento en relación a la rotación del personaje
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Aplica el movimiento al Rigidbody
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

        // Verifica si el jugador está en el suelo antes de permitir el salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Aplica una fuerza de salto
            isGrounded = false; // Actualiza el estado de "en el suelo"
        }
    }

    // Detecta si el personaje aterriza en el suelo
    private void OnCollisionEnter(Collision collision)
    {
        // Solo permite el salto si colisiona con el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Actualiza a "en el suelo" cuando toca el suelo
        }
    }

    // Método para controlar el disparo de balas
    private void HandleShooting()
    {
        // Disparar una bala con el clic izquierdo del ratón
        if (Input.GetMouseButtonDown(0))
        {
            GameObject buf = Instantiate(bullet); // Crea una instancia de la bala
            buf.transform.position = rifleStart.position; // Posiciona la bala en la posición inicial
            buf.GetComponent<Bullet>().setDirection(transform.forward); // Establece la dirección de la bala
            buf.transform.rotation = transform.rotation; // Alinea la rotación de la bala con el jugador
        }
    }

    // Método para manejar acciones cercanas a objetos específicos
    private void HandleNearbyActions()
    {
        // Destruir enemigos cercanos con el clic derecho del ratón
        if (Input.GetMouseButtonDown(1))
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in enemies)
            {
                if (item.CompareTag("Enemy"))
                {
                    Destroy(item.gameObject); // Destruye el enemigo
                    IncreaseEnemyCount(); // Aumenta el contador de enemigos eliminados
                }
            }
        }
    }

    // Método para aumentar el contador de enemigos y verificar la victoria
    public void IncreaseEnemyCount()
    {
        enemiesKilled++;
        if (enemiesKilled >= totalEnemies)
        {
            Win(); // Llama al método Win si todos los enemigos han sido eliminados
        }
    }
}
