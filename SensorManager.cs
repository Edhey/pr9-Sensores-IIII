using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Text;

public class SensorManager : MonoBehaviour {
    [SerializeField] private TMP_Text infoText;

    private StringBuilder _sb = new StringBuilder();

    void Start() {
        // Listamos los sensores disponibles en la consola para depuración
        Debug.Log("Sensores Disponibles:");
        foreach (var device in InputSystem.devices) {
            Debug.Log($"- {device.displayName} ({device.description})");
        }

        // Habilitamos los sensores que nos interesan
        EnableSensor(Accelerometer.current);
        EnableSensor(UnityEngine.InputSystem.Gyroscope.current);
        EnableSensor(GravitySensor.current);
        EnableSensor(AttitudeSensor.current);
        EnableSensor(LinearAccelerationSensor.current);
        EnableSensor(MagneticFieldSensor.current);
        EnableSensor(LightSensor.current);
        EnableSensor(PressureSensor.current);
        EnableSensor(ProximitySensor.current);
        EnableSensor(HumiditySensor.current);
        EnableSensor(AmbientTemperatureSensor.current);
        EnableSensor(StepCounter.current);

    }

    void Update() {
        if (infoText == null)
            return;

        _sb.Clear();
        _sb.AppendLine("<size=120%><b>--- PANEL DE SENSORES ---</b></size>\n");

        // 1. Acelerómetro (Movimiento + Gravedad)
        if (Accelerometer.current != null && Accelerometer.current.enabled) {
            Vector3 val = Accelerometer.current.acceleration.ReadValue();
            _sb.AppendLine($"<b>Acelerómetro:</b> <color=#FFD700>{val:F2}</color> g");
        }
        else
            _sb.AppendLine("Acelerómetro: <color=grey>No Disp.</color>");

        // 2. Aceleración Lineal (Movimiento puro, sin gravedad)
        if (LinearAccelerationSensor.current != null && LinearAccelerationSensor.current.enabled) {
            Vector3 val = LinearAccelerationSensor.current.acceleration.ReadValue();
            _sb.AppendLine($"<b>Acel. Lineal:</b> <color=#FFD700>{val:F2}</color> g");
        }
        else
            _sb.AppendLine("Acel. Lineal: <color=grey>No Disp.</color>");

        // 3. Gravedad (Solo vector gravedad)
        if (GravitySensor.current != null && GravitySensor.current.enabled) {
            Vector3 val = GravitySensor.current.gravity.ReadValue();
            _sb.AppendLine($"<b>Gravedad:</b> <color=#87CEEB>{val:F2}</color> m/s²");
        }
        else
            _sb.AppendLine("Gravedad: <color=grey>No Disp.</color>");

        // 4. Giroscopio (Velocidad Angular)
        if (UnityEngine.InputSystem.Gyroscope.current != null && UnityEngine.InputSystem.Gyroscope.current.enabled) {
            Vector3 val = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.ReadValue();
            _sb.AppendLine($"<b>Giroscopio:</b> <color=#98FB98>{val:F2}</color> rad/s");
        }
        else
            _sb.AppendLine("Giroscopio: <color=grey>No Disp.</color>");

        // 5. Actitud / Orientación (Cuaterniones convertidos a Euler para legibilidad)
        if (AttitudeSensor.current != null && AttitudeSensor.current.enabled) {
            Quaternion q = AttitudeSensor.current.attitude.ReadValue();
            Vector3 euler = q.eulerAngles;
            _sb.AppendLine($"<b>Attitude (Euler):</b> <color=#FFA07A>({euler.x:F0}, {euler.y:F0}, {euler.z:F0})</color>°");
        }
        else
            _sb.AppendLine("Attitude: <color=grey>No Disp.</color>");

        // 6. Magnetómetro
        if (MagneticFieldSensor.current != null && MagneticFieldSensor.current.enabled) {
            Vector3 val = MagneticFieldSensor.current.magneticField.ReadValue();
            _sb.AppendLine($"<b>Magnetómetro:</b> <color=#DDA0DD>{val:F1}</color> µT");
        }
        else
            _sb.AppendLine("Magnetómetro: <color=grey>No Disp.</color>");

        // 7. Luz
        if (LightSensor.current != null && LightSensor.current.enabled) {
            float val = LightSensor.current.lightLevel.ReadValue();
            _sb.AppendLine($"<b>Luz:</b> <color=#FFFFE0>{val:F1}</color> lux");
        }
        else
            _sb.AppendLine("Luz: <color=grey>No Disp.</color>");

        // 8. Proximidad
        if (ProximitySensor.current != null && ProximitySensor.current.enabled) {
            // Nota: Muchos móviles solo devuelven 0 (cerca) o 5/max (lejos), es binario.
            float val = ProximitySensor.current.distance.ReadValue();
            _sb.AppendLine($"<b>Proximidad:</b> <color=#FF69B4>{val:F1}</color> cm");
        }
        else
            _sb.AppendLine("Proximidad: <color=grey>No Disp.</color>");

        // 9. Contador de Pasos
        if (StepCounter.current != null && StepCounter.current.enabled) {
            int pasos = StepCounter.current.stepCounter.ReadValue();
            _sb.AppendLine($"<b>Pasos:</b> <color=#FF4500>{pasos}</color>");
        }
        else
            _sb.AppendLine("Pasos: <color=grey>No Disp.</color>");

        infoText.text = _sb.ToString();
    }

    private void EnableSensor(InputDevice sensor) {
        if (sensor != null) {
            InputSystem.EnableDevice(sensor);
        }
    }
}