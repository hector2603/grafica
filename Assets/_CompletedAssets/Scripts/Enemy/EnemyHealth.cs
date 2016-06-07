using UnityEngine;

namespace CompleteProject
{
    public class EnemyHealth : MonoBehaviour
    {
        public int startingHealth = 100;            // cantidad inicial de salud del enemigo
        public int currentHealth;                   // salud actual del enemigo
        public float sinkSpeed = 2.5f;              // la velocidad con que el enemigo desaparece a traves del suelo cuando muere.
        public int scoreValue = 10;                 // la cantidad de puntuacion añadida al jugador cuando el enemigo muere
        public AudioClip deathClip;                 // el sonido cuando un enemigo muere


        Animator anim;                              // referenete a la animacion
        AudioSource enemyAudio;                     // referente a la fuente de audio.
        ParticleSystem hitParticles;                // referencia al sistema de particulas que se produce cuando el enemigo es dañado.
        CapsuleCollider capsuleCollider;            // referencia a la capsula de colisionador 
        bool isDead;                                // enemigo muerto
        bool isSinking;                             // el enemigo empieza a undirse en el suelo


        void Awake ()
        {
            // configuracion de referencias
            anim = GetComponent <Animator> ();
            enemyAudio = GetComponent <AudioSource> ();
            hitParticles = GetComponentInChildren <ParticleSystem> ();
            capsuleCollider = GetComponent <CapsuleCollider> ();

            // ajuste de salud cuando los primeros enemigos nacen
            currentHealth = startingHealth;
        }


        void Update ()
        {
            // el enemigo debe undirse en el suelo...
            if(isSinking)
            {
                // ... mover al enemigo a  sinkSpeed en el tiempo determinado.
                transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        }


        public void TakeDamage (int amount, Vector3 hitPoint)
        {
            // si el enemigo esta muerto...
            if(isDead)
                // ... no necesita hacer daños entonces sale de la funcion.
                return;

            // reproducir sonido de herido.
            enemyAudio.Play ();

            // reduce la cantidad de salud segun los daños sufridos
            currentHealth -= amount;
            
            // establecer la posicion de  particulas al que se sufrio el golpe.
            hitParticles.transform.position = hitPoint;

            // reproducir particulas.
            hitParticles.Play();

            // si la salud actual es menor o igual a cero...
            if(currentHealth <= 0)
            {
                // ... el enemigo esta muerto
                Death ();
            }
        }


        void Death ()
        {
            // el enemigo esta muerto
            isDead = true;

            // gire el colisionador de impacto de forma que el disparo pase a traves 
            capsuleCollider.isTrigger = true;

            // decir que el enemigo esta muerto
            anim.SetTrigger ("Dead");

			// cambiar el audio por el efecto de muerto (esto detiene el efecto de herido)
            enemyAudio.clip = deathClip;
            enemyAudio.Play ();
        }


        public void StartSinking ()
        {
            // desactivar la malla de navegacion del agente
            GetComponent <NavMeshAgent> ().enabled = false;

            // encontrar el cuerpo rigido y empezar a undirlo
            GetComponent <Rigidbody> ().isKinematic = true;

            // el enemigo debe undirse
            isSinking = true;

            // aumentar la puntuacion por el enemigo
            ScoreManager.score += scoreValue;

            // despues de dos segundos destruir al enemigo.
            Destroy (gameObject, 2f);
        }
    }
}