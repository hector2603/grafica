using UnityEngine;
using UnityEngine;

namespace CompleteProject
{
	public class PlayerShooting : MonoBehaviour
	{
		public int damagePerShot = 20;                  // The damage inflicted by each bullet.
		public float timeBetweenBullets = 0.15f;        // el tiempo entre cada disparo
		public float range = 100f;                      // The distance the gun can fire.


		float timer;                                    // A timer to determine when to fire.
		Ray shootRay;                                   // A ray from the gun end forwards.
		RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
		int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
		ParticleSystem gunParticles;                    // Reference to the particle system.
		LineRenderer gunLine;                           // Reference to the line renderer.
		LineRenderer gunLineCenter;
		AudioSource gunAudio;                           // Reference to the audio source.
		Light gunLight;                                 // Reference to the light component.
		public Light faceLight;								// Duh
		float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
		public int lengthOfLineRenderer = 20;// agregueeeee, es el número de lineas que va a tener la onda, si se quiere más bonita, hay que colocarle mas 


		void Awake ()
		{
			// Create a layer mask for the Shootable layer.
			shootableMask = LayerMask.GetMask ("Shootable");

			// Set up the references.
			gunParticles = GetComponent<ParticleSystem> ();
			gunLine = GetComponent <LineRenderer> ();
			gunLineCenter = GetComponent <LineRenderer> ();
			gunAudio = GetComponent<AudioSource> ();
			gunLight = GetComponent<Light> ();
			gunLine.SetVertexCount (lengthOfLineRenderer); // aquiiiiiiiiii, asigne el numero de vertices que va a tener la linea
			//faceLight = GetComponentInChildren<Light> ();
		}


		void Update ()
		{
			// Add the time since Update was last called to the timer.
			timer += Time.deltaTime;

			#if !MOBILE_INPUT
			// If the Fire1 button is being press and it's time to fire...
			if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
			{
				// ... shoot the gun.
				Shoot ();
			}
			#else
			// If there is input on the shoot direction stick and it's time to fire...
			if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets)
			{
			// ... shoot the gun
			Shoot();
			}
			#endif
			// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
			if(timer >= timeBetweenBullets * effectsDisplayTime)
			{
				// ... disable the effects.
				DisableEffects ();
			}
		}


		public void DisableEffects ()
		{
			// Disable the line renderer and the light.
			gunLine.enabled = false;
			faceLight.enabled = false;
			gunLight.enabled = false;
			gunLineCenter.enabled = false;

		}

		/* La función es la encargada realizar
		 * el disparo del jugador, el cual se
		 * encargda de emplear la trayectoria de
		 * la bala con la función seno, y se 
		 * genera el daño por bala por medio de 
		 * la función coseno.
		 */
		void Shoot ()
		{
			// Reset the timer.
			timer = 0f;

			// Play the gun shot audioclip.
			gunAudio.Play ();

			// Enable the lights.
			gunLight.enabled = true;
			faceLight.enabled = true;

			// Stop the particles from playing if they were, then start the particles.
			gunParticles.Stop ();
			gunParticles.Play ();

			// Enable the line renderer and set it's first position to be the end of the gun.
			gunLine.enabled = true;
			gunLineCenter.enabled = true;

			// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
			shootRay.origin = transform.position;
			shootRay.direction = transform.forward;

			// Perform the raycast against gameobjects on the shootable layer and if it hits something...
			if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
			{
				// Try and find an EnemyHealth script on the gameobject hit.
				EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

				//PARA GENERAR DAÑO ALEATORIO
				int danoPorBala = 30; //Inicializamos el valor de daño por cada bala en 30
				if (Application.loadedLevelName == "level2") {  //Si se encuentra en el nivel2 el daño por bala va a ser siempre 100
					damagePerShot = 100;  //Toma el valor de 100
				} else { // si estamos en el nivel1 
					//Aplicamos la función cosenos para asignar daño por bala
					float funcionCos = Mathf.Cos (Random.Range (-Mathf.PI / 2, Mathf.PI / 2));             
					int danoF = (int)(funcionCos * 100); //Multiplixamos por 100 dado a que la función Coseno toma valores entre [0-1]
					if (danoF <= danoPorBala) { //Si es menor a 30 entonces asignamos el número aleatorio a daño por bala
						damagePerShot = danoF;//se asigna el valor al la variable daño
					} else { //Si es  mayor a 30 asignamos 30 a daño por bala
						damagePerShot = danoPorBala;
					}
				}
				//Fin generar daño aleatorio

				// If the EnemyHealth component exist...
				if(enemyHealth != null)
				{
					// ... the enemy should take damage.
					enemyHealth.TakeDamage (damagePerShot, shootHit.point);
				}

				// Set the second position of the line renderer to the point the raycast hit.			
				Vector3 dir = (shootHit.point-transform.position).normalized;
				Vector3[] points = new Vector3[lengthOfLineRenderer];// se crea un arreglo de vectores de 3 dimensiones, donde se guardara cada vertice de la linea
				float t = Time.time;
				float tamanoDivisiones = (shootHit.point - transform.position).magnitude/lengthOfLineRenderer;
				int i = 1;
				points [0] = transform.position;
				while (i < lengthOfLineRenderer) {
					points[i] = new Vector3((i*tamanoDivisiones*dir.x)+transform.position.x,
											shootHit.point.y,
											(Mathf.Sin(i + t))+(i*tamanoDivisiones*dir.z)+transform.position.z);
					i++;
				}
				gunLine.SetPositions(points);// crea una linea por todos los vertices guardados en el arreglo 
			}
			// If the raycast didn't hit anything on the shootable layer...
			else
			{
				// ... set the second position of the line renderer to the fullest extent of the gun's range.
				gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
			}
		}
	}
}