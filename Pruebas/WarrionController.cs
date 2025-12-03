using UnityEngine;
using System.Collections;

public class WarriorController : MonoBehaviour {
    [Header("Configuración GPS (Tenerife/Canarias)")]
    // Ajustado a tu ubicación aprox de la foto
    public float minLat = 28.000f;
    public float maxLat = 29.000f;
    public float minLon = -17.000f;
    public float maxLon = -16.000f;

    [Header("Movimiento")]
    public float velocidadBase = 5f;
    public float suavizadoRotacion = 5f; // Más alto = más reactivo

    private bool gpsActivo = false;
    private string estadoGeofence = "Esperando...";

    IEnumerator Start() {
        // Forzar Landscape Left tal y como pide el enunciado
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Input.location.Start();

        // Necesario IEnumerator para esperar a que el GPS arranque
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed || maxWait < 1) {
            estadoGeofence = "Error: GPS Falló";
            Debug.LogError(estadoGeofence);
        }
        else {
            Input.compass.enabled = true;
            gpsActivo = true;
        }
    }

    void Update() {
        if (!gpsActivo)
            return;

        // GEOFENCING
        float lat = Input.location.lastData.latitude;
        float lon = Input.location.lastData.longitude;
        bool dentroDeRango = (lat >= minLat && lat <= maxLat) && (lon >= minLon && lon <= maxLon);

        if (!dentroDeRango) {
            estadoGeofence = "FUERA DE RANGO";
            return;
        }
        estadoGeofence = "EN ZONA";

        // ORIENTACIÓN
        float camaraY = Camera.main.transform.eulerAngles.y;
        float brujula = Input.compass.trueHeading;
        // Al girar el móvil a la izquierda, el sensor se desalinea 90 grados.
        float brujulaCorregida = brujula + 90f;

        // "Norte = Dónde mira mi cámara - Dónde dice la brújula que está el norte"
        float norteEnUnity = camaraY - brujulaCorregida;

        // 4. Aplicar rotación
        Quaternion rotacionObjetivo = Quaternion.Euler(0, norteEnUnity, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * suavizadoRotacion);


        // MOVIMIENTO (Acelerómetro Z Invertido)
        float inputZ = -Input.acceleration.z;
        Vector3 movimiento = transform.forward * inputZ * velocidadBase * Time.deltaTime;
        transform.Translate(movimiento, Space.World);
    }

    void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.fontSize = 40;
        GUILayout.BeginArea(new Rect(50, 50, Screen.width, Screen.height));
        GUILayout.Label($"Geofence: {estadoGeofence}", style);

        style.normal.textColor = Color.white;
        GUILayout.Label($"Lat: {Input.location.lastData.latitude:F4}", style);
        // Debug de la matemática de orientación
        GUILayout.Label($"Cam Y: {Camera.main.transform.eulerAngles.y:F0}", style);
        GUILayout.Label($"Brújula: {Input.compass.trueHeading:F0}", style);
        GUILayout.Label($"Objetivo Y: {(Camera.main.transform.eulerAngles.y - (Input.compass.trueHeading + 90)):F0}", style);
        GUILayout.Label($"Accel Z inv: {-Input.acceleration.z:F2}", style);
        GUILayout.EndArea();
    }
}