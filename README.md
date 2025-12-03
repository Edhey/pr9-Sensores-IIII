# Práctica 10 - Monitor de Sensores

- **Author**: Himar Edhey Hernández Alonso
- **Subject**: Interfaces Inteligentes

## Introducción

En esta práctica se han estudiado los disntintos sensores disponibles en un dispositivo móvil Android, así como su uso en Unity.

## Sensores

*Crear una aplicación en Unity que muestre en la UI los valores de todos los sensores disponibles en tu móvil. Incluir en el Readme una medida de los valores en el laboratorio y otra en el jardin de la ESIT.*

En este caso se ha creado un script que permite ver los sensores disponibles en el dispositivo. Luego, se han habilitado los mismos para obtener sus datos en tiempo real y mostrarlos en pantalla.

[Sensores en Unity](SensorManager.cs)

### Sensores Implementados

Al hacer debug de los sensores disponibles en el dispositivo, se pueden observar los siguientes:

![Lista Sensores](Resources/lista_sensores.png)
![Lista Sensores](Resources/lista_sensores-1.png)

Por lo tanto, se han implementado los siguientes sensores en la aplicación:

- Acelerómetro: Aceleración bruta (incluye gravedad).
- Aceleración Lineal: Aceleración del usuario (excluye gravedad).
- Gravedad: Vector de gravedad aislado.
- Giroscopio: Velocidad angular de rotación.
- Attitude (Orientación): Rotación absoluta del dispositivo en el espacio.
- Magnetómetro: Campo magnético terrestre (brújula).
- Sensor de Luz: Iluminancia ambiental.
- Proximidad: Distancia a objetos cercanos (sensor frontal).
- Contador de Pasos: Número de pasos dados por el usuario.

## Tabla de Mediciones

A continuación se comparan los valores obtenidos en dos entornos con características físicas distintas:

1. **Laboratorio:** Entorno interior controlado, luz artificial, posibles interferencias magnéticas.

![Mediciones Laboratorio](Resources/sensores-laboratorio.jpg)

| Sensor | Valor | Unidad |
| :--- | :--- | :--- |
| **Acelerómetro** | `0.08, -0.64, -0.70` | g |
| **Acel. Lineal** | `0.00, 0.00, 0.07` | g |
| **Gravedad** | `0.09, -0.63, -0.77` | m/s² |
| **Giroscopio** | `-0.10, -0.13, 0.02` | rad/s |
| **Attitude (Euler)**| `40, 4, 358` | ° (grados) |
| **Magnetómetro** | `-28.0, -10.7, -22.7` | µT |
| **Luz** | `458,0` | lux |
| **Proximidad** | `5,0` | cm |
| **Pasos** | `0` | pasos |

2. **Jardín:** Entorno exterior, luz natural, campo magnético terrestre más limpio.

![Mediciones Jardín](Resources/sensores-jardin.jpg)

| Sensor | Valor | Unidad |
| :--- | :--- | :--- |
| **Acelerómetro** | `0.08, -0.64, -0.70` | g |
| **Acel. Lineal** | `0.00, 0.00, 0.07` | g |
| **Gravedad** | `0.09, -0.63, -0.77` | m/s² |
| **Giroscopio** | `-0.10, -0.13, 0.02` | rad/s |
| **Attitude (Euler)**| `40, 4, 358` | ° (grados) |
| **Magnetómetro** | `-28.0, -10.7, -22.7` | µT |
| **Luz** | `458,0` | lux |
| **Proximidad** | `5,0` | cm |
| **Pasos** | `0` | pasos |

## Orientación con GPS

*Crear una apk que oriente alguno de los guerreros de la práctica mirando siempre hacia el norte, avance con una aceleración proporcional a la del dispositivo y lo pare cuando el dispositivo esté fuera de un rango de latitud, longitud dado. El acelerómetro nos dará la velocidad del movimiento. A lo largo del eje z (hacia adelante y hacia atrás), se produce el movimiento inclinando el dispositivo hacia adelante y hacia atrás. Sin embargo, necesitamos invertir el valor z porque la orientación del sistema de coordenadas corresponde con el punto de vista del dispositivo. Queremos que la rotación final coincida con la orientación cuando mantenemos el dispositivo en la posición Horizontal Izquierda. Esto ocurre cuando la izquierda en la posición vertical ahora es la parte inferior. Aplicar las rotaciones con interpolación  Slerp en un quaternion.*

Para lograr crear esta apk primero añadí en una escena nueva el guerrero a controlar. Luego, creé un script que permitiera obtener la orientación del dispositivo y mover al guerrero hacia el norte.

[Face North](FaceNorth.cs)

Por último, se limitó el movimiento del guerrero a un rango específico de latitud y longitud. Así como se añadió una aceleración proporcional a la inclinación del dispositivo.

[Warrior Movement](WarriorMovement.cs)

Para ambos es necesario el uso del GPS, se usa un IEnumerator para esperar a que el servicio de localización esté habilitado y obtener las coordenadas. Ajustamos la latitud y longitud permitida para el movimiento del guerrero a la zona de Tenerife y movemos el guerrero solo si el dispositivo está dentro de ese rango. Para el movimiento, se obtiene la inclinación del dispositivo a lo largo del eje z y se aplica una fuerza proporcional a esa inclinación para mover al guerrero hacia adelante o atrás, tal y como se describe en el enunciado. Además, se utiliza Slerp para suavizar la rotación del guerrero hacia el norte.

Hemos creado una GUI que muestra la latitud y longitud actuales del dispositivo, así como un mensaje que indica si el guerrero puede moverse o no según su ubicación para hacer debugging, por ello queda comentado.
