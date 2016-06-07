using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CompleteProject
{
    public class PlayerHealth : MonoBehaviour
    {
        public int startingHealth = 100;                            // la cantidad de salud con la que empieza el jugador
        public int currentHealth;                                   // salud actual del jugador.
        public Slider healthSlider;                                 // referecia a la barra de salud del jugador
        public Image damageImage;                                   // referencia al parpadeo o flash que se da cuando es herido.
        public AudioClip deathClip;                                 // audio cuando el jugador muere.
        public float flashSpeed = 5f;                               // velocidad del parpadeo o flash
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // color del parpadeo o flash.


        Animator anim;                                              // referencia al componente animado.
        AudioSource playerAudio;                                    // referente a la fuente de audio.
        PlayerMovement playerMovement;                              // referencia a los movimientos del jugador
        PlayerShooting playerShooting;                              // referencia a los disparos.
        bool isDead;                                                //si el jugador esta muerto.
        bool damaged;                                               // verdad cuando el jugador es herido


        void Awake ()
        {
            // configuracion de las referencias
            anim = GetComponent <Animator> ();
            playerAudio = GetComponent <AudioSource> ();
            playerMovement = GetComponent <PlayerMovement> ();
            playerShooting = GetComponentInChildren <PlayerShooting> ();

            // configuracion de la salud inicial del jugador
            currentHealth = startingHealth;
        }


        void Update ()
        {
            // si el jugador esta siendo herido...
            if(damaged)
            {
                // ... muestre el flash.
                damageImage.color = flashColour;
            }
            // sino..
            else
            {
                // ... color normal.
                damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }

            // resetear indicador de daño.
            damaged = false;
        }


        public void TakeDamage (int amount)
        {
            // indicador de daño entonces habra parpadeo.
            damaged = true;

            // reduce la cantidad de salud segun las heridas
            currentHealth -= amount;

            // pone la barra de salud en la cantidad actual.
            healthSlider.value = currentHealth;

            // reproducir efecto de daño
            playerAudio.Play ();

            // si el jugador a muerto y el indicador no coincide con que esta muerto...
            if(currentHealth <= 0 && !isDead)
            {
                // ...debe morir.
                Death ();
            }
        }


        void Death ()
        {
            // si esta muerto esta funcion no se llama de nuevo
            isDead = true;

            // desactivar cualquier efecto de sonido.
            playerShooting.DisableEffects ();

            // mostrar que ha muerto.
            anim.SetTrigger ("Die");

            // reproduce el sonido de muerte y desactiva el de herida.
            playerAudio.clip = deathClip;
            playerAudio.Play ();

            // desactiva movimientos y disparos
            playerMovement.enabled = false;
            playerShooting.enabled = false;
        }


        public void RestartLevel ()
        {
            // actualiza la escena actual
            SceneManager.LoadScene (0);
        }
    }
}