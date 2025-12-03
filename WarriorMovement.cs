using UnityEngine;
using System.Collections;

public class WarriorMovement : MonoBehaviour {
    [SerializeField] private float minLat = 28.000f;
    [SerializeField] private float maxLat = 29.000f;
    [SerializeField] private float minLon = -17.000f;
    [SerializeField] private float maxLon = -16.000f;

    [SerializeField] private float velocidadBase = 5f;
    public static bool gpsActivo { get; private set; } = false;

    private string estadoDebug = "Iniciando GPS...";

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
            estadoDebug = "Error: GPS Falló";
            Debug.LogError(estadoDebug);
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
            estadoDebug = "FUERA DE RANGO";
            return;
        }
        estadoDebug = "EN ZONA";

        // MOVIMIENTO (Acelerómetro Z Invertido)
        float inputZ = -Input.acceleration.z;
        Vector3 movimiento = transform.forward * inputZ * velocidadBase * Time.deltaTime;
        transform.Translate(movimiento, Space.World);
    }

    // void OnGUI() {
    //     // UI de debug
    //     GUIStyle style = new GUIStyle();
    //     style.fontSize = 40;
    //     style.normal.textColor = estadoDebug.Contains("FUERA DE RANGO") ? Color.red : Color.green;

    //     GUILayout.BeginArea(new Rect(50, 50, Screen.width, Screen.height));
    //     GUILayout.Label($"Estado: {estadoDebug}", style);

    //     if (gpsActivo) {
    //         style.normal.textColor = Color.white;
    //         GUILayout.Label($"Lat: {Input.location.lastData.latitude:F4}", style);
    //         GUILayout.Label($"Lon: {Input.location.lastData.longitude:F4}", style);
    //         GUILayout.Label($"Velocidad (Z inv): {-Input.acceleration.z:F2}", style);
    //     }
    //     GUILayout.EndArea();
    // }
}