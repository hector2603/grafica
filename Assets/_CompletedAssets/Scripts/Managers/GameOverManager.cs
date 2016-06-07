using UnityEngine;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // referente a la salud del jugador


        Animator anim;                          // referente al componente animado


        void Awake ()
        {
            // configuracion de las referencias
            anim = GetComponent <Animator> ();
        }


        void Update ()
        {
            // si el jugador se queda sin salud...
            if(playerHealth.currentHealth <= 0)
            {
                // ... decir que el juego ha terminado.
                anim.SetTrigger ("GameOver");
            }
        }
    }
}