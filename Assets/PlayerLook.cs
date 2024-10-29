using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSense; // Sensibilidad del movimiento del mouse
    [SerializeField] Transform player, playerArms; // Referencias al jugador y a los brazos (o cámara) del jugador

    float xAxisClamp = 0; // Limite de rotación en el eje X para evitar rotación completa vertical

    // Método que se llama una vez por frame
    void Update()
    {
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;

        // Obtener el movimiento del mouse en los ejes X e Y y aplicar la sensibilidad
        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;

        // Actualizar el valor del clamp para el eje X
        xAxisClamp -= rotateX;

        // Obtener los ángulos actuales de rotación para el jugador y los brazos
        Vector3 rotPlayerArms = playerArms.rotation.eulerAngles;
        Vector3 rotPlayer = player.rotation.eulerAngles;

        // Aplicar el movimiento vertical al eje X de los brazos y resetear el eje Z
        rotPlayerArms.x -= rotateY;
        rotPlayerArms.z = 0;

        // Aplicar el movimiento horizontal al eje Y del jugador
        rotPlayer.y += rotateX;

        // Limitar la rotación vertical (para evitar que el jugador mire completamente hacia arriba o abajo)
        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            rotPlayerArms.x = 90; // Límite superior
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            rotPlayerArms.x = 270; // Límite inferior
        }

        // Aplicar las rotaciones calculadas a los brazos y al jugador
        playerArms.rotation = Quaternion.Euler(rotPlayerArms);
        player.rotation = Quaternion.Euler(rotPlayer);
    }
}
