using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // segundos que pasan entre cada ataque.
        public int attackDamage = 10;               // cantidad de salud que quita el ataque.


        Animator anim;                              // componente animado
        GameObject player;                          // jugador o sea objetivo
        PlayerHealth playerHealth;                  // salud del jugador.
        EnemyHealth enemyHealth;                    // salud del enemigo.
        bool playerInRange;                         // el jugador esta dentro de rango de disparo
        float timer;                                // tiempo que pasa para el siguiente ataque .


        void Awake ()
        {
            // configuracion de las referencias.
            player = GameObject.FindGameObjectWithTag ("Player");
            playerHealth = player.GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent<EnemyHealth>();
            anim = GetComponent <Animator> ();
        }


        void OnTriggerEnter (Collider other)
        {
            // si el jugador esta en el rango de disparo...
            if(other.gameObject == player)
            {
                // ... jugador si esta en el rango.
                playerInRange = true;
            }
        }


        void OnTriggerExit (Collider other)
        {
            // si el jugador esta fuera del rango de disparo...
            if(other.gameObject == player)
            {
                // ... jugador no esta en el rango
                playerInRange = false;
            }
        }


        void Update ()
        {
            // temporizador
            timer += Time.deltaTime;

            // si el temporizador supera los tiempos entre ataques, el jugador esta en el rango de disparo y el enemigo esta vivo
            if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
            {
                // ... ataque.
                Attack ();
            }

            // si el jugador tiene salud cero o menos...
            if(playerHealth.currentHealth <= 0)
            {
                // ... mostrar "jugador muerto".
                anim.SetTrigger ("PlayerDead");
            }
        }


        void Attack ()
        {
            // reinicie tiempos.
            timer = 0f;

            // si el jugador tiene salus por perder...
            if(playerHealth.currentHealth > 0)
            {
                // ... dañar al jugador
                playerHealth.TakeDamage (attackDamage);
            }
        }
    }
}