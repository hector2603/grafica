using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // referente a la posicion del jugador.
        PlayerHealth playerHealth;      // referente a la salud del jugador.
        EnemyHealth enemyHealth;        // referente a la salud del enemigo.
        NavMeshAgent nav;               // referente a la malla de navegador.


        void Awake ()
        {
            // configuracion de las referencias
            player = GameObject.FindGameObjectWithTag ("Player").transform;
            playerHealth = player.GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent <EnemyHealth> ();
            nav = GetComponent <NavMeshAgent> ();
        }


        void Update ()
        {
            // si el enemigo y jugador tiene salud mayor a cero...
            if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {

                nav.SetDestination (player.position);
            }
            // otra manera..
            else
            {
                // ...desactivar malla de navegacion.
                nav.enabled = false;
            }
        }
    }
}