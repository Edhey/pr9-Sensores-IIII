using UnityEngine;

public class FaceNorth : MonoBehaviour {
    [Header("Configuración Orientación")]
    public float suavizadoRotacion = 5f;

    void Start() {
        Input.compass.enabled = true;
    }

    void Update() {
        if (Input.location.status != LocationServiceStatus.Running)
            return;
        // ORIENTACIÓN
        float camaraY = Camera.main.transform.eulerAngles.y;
        float brujula = Input.compass.trueHeading; // el gps debe estar activo
        // Al girar el móvil a la izquierda, el sensor se desalinea 90 grados.
        float brujulaCorregida = brujula;
        // Norte = Dónde mira mi cámara - Dónde dice la brújula que está el norte
        float norteEnUnity = camaraY - brujulaCorregida;

        // ROTACIÓN
        Quaternion rotacionObjetivo = Quaternion.Euler(0, norteEnUnity, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * suavizadoRotacion);
    }
}