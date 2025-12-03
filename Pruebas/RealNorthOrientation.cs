using UnityEngine;
using System.Collections;

public class RealNorthOrientation : MonoBehaviour {
    [Header("Configuración")]
    public bool usarInterpolacion = true;
    public float velocidadGiro = 5f;

    // Variables de estado para debug
    string estadoDebug = "Iniciando...";
    float ultimaLecturaBrujula = 0;
    float ultimaRotacionCamara = 0;
    float rotacionCalculada = 0;

    IEnumerator Start() {
        // 1. Solicitar permisos y encender servicios
        estadoDebug = "Solicitando servicios...";
        Debug.Log("Iniciando servicio de localización...");

        Input.location.Start();

        // 2. Esperar a la inicialización (Time-out de 20 segundos)
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // 3. Comprobar fallos
        if (maxWait < 1) {
            estadoDebug = "Error: Tiempo de espera agotado (Time out).";
            Debug.LogError(estadoDebug);
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed) {
            estadoDebug = "Error: No se pudo determinar la ubicación del dispositivo.";
            Debug.LogError(estadoDebug);
            yield break;
        }

        // 4. Si llegamos aquí, tenemos acceso. Encendemos la brújula.
        Input.compass.enabled = true;
        estadoDebug = "Servicios activos. Esperando datos...";
        Debug.Log("Servicios de localización y brújula activos.");
    }

    void Update() {
        // Si el servicio no está corriendo, no hacemos nada
        if (Input.location.status != LocationServiceStatus.Running)
            return;

        // Recogemos datos para debug
        ultimaLecturaBrujula = Input.compass.trueHeading;
        ultimaRotacionCamara = Camera.main.transform.eulerAngles.y;

        // Cálculo del norte para el objeto
        // Objeto en suelo: Restamos la brújula a la cámara para mantener el norte fijo
        float norteEnUnity = ultimaRotacionCamara - ultimaLecturaBrujula;
        rotacionCalculada = norteEnUnity;

        Quaternion rotacionObjetivo = Quaternion.Euler(0, norteEnUnity, 0);

        // Aplicamos rotación
        if (usarInterpolacion) {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadGiro);
        }
        else {
            transform.rotation = rotacionObjetivo;
        }
    }

    // Dibuja los datos en la pantalla del móvil (modo rápido sin Canvas)
    void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.fontSize = 40; // Grande para ver en móvil
        style.normal.textColor = Color.red;

        GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height - 100));

        GUILayout.Label("--- DIAGNÓSTICO BRÚJULA ---", style);
        GUILayout.Label($"Estado: {estadoDebug}", style);
        GUILayout.Label($"Location Status: {Input.location.status}", style);
        GUILayout.Label($"Compass Enabled: {Input.compass.enabled}", style);
        GUILayout.Space(20);
        GUILayout.Label($"Brújula (TrueHeading): {ultimaLecturaBrujula:F2}°", style);
        GUILayout.Label($"Cámara Y: {ultimaRotacionCamara:F2}°", style);
        GUILayout.Label($"Target Rotación Y: {rotacionCalculada:F2}°", style);

        // Advertencia si estamos en Editor
        if (Application.isEditor) {
            style.normal.textColor = Color.yellow;
            GUILayout.Label("AVISO: Estás en Unity Editor.\nLa brújula no funciona aquí sin Unity Remote.", style);
        }

        GUILayout.EndArea();
    }
}