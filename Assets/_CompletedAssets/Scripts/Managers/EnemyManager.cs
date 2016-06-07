using UnityEngine;

namespace CompleteProject
{
    public class EnemyManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // referente a la salud del jugador
        public GameObject enemy;                // se genera el enemigo
        public float spawnTime = 3f;            // tiempo entre cada nacimiento
        public Transform[] spawnPoints;         // arreglo de los puntos de nacimiento


        void Start ()
        {
            //llamado de la funcion para la creacion de enemigos en el los tiempos establecidos
            InvokeRepeating ("Spawn", spawnTime, spawnTime);
        }


        void Spawn ()
        {
            // si el jugador tiene la salud menor a cero
            if(playerHealth.currentHealth <= 0f)
            {
                // ... salir de la funcion.
                return;
            }

            // encontrar un numero ramdon entre cero y los puntos de nacimientos
            int spawnPointIndex = Random.Range (0, spawnPoints.Length);

            // se selecciona al azar la posicion y rotacion de la creacion del enemigo
            Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
    }
}