using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 6f;            // velocidad en que puede moverse el jugador


        Vector3 movement;                   // vector para almacenar los movimientos del jugador
        Animator anim;                      // referente al componente animado
        Rigidbody playerRigidbody;          // referencia al cuerpo rigido o muerto
#if !MOBILE_INPUT
        int floorMask;                      // mascara del suelo
        float camRayLength = 100f;          // longitud del rayo de la camara en la escena 
#endif

        void Awake ()
        {
#if !MOBILE_INPUT
            // crear una mascara para la capa baja "suelo".
            floorMask = LayerMask.GetMask ("Floor");
#endif

            // configuracion de referencias
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();
        }


        void FixedUpdate ()
        {
            // almacenar los ejes de entrada
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // movimiento del jugador alrededor de la escena

            Move (h, v);

            // girar al jugador con los movimientos del cursor
            Turning ();

            // animacion del jugador
            Animating (h, v);
        }


        void Move (float h, float v)
        {
            // establecer el vector de movimiento segun en eje de entrada
            movement.Set (h, 0f, v);
            
            // normalizar el vector de movimiento proporcional a la velocidad.
            movement = movement.normalized * speed * Time.deltaTime;

            // mover al jugador a su posicion actual mas su movimiento.
            playerRigidbody.MovePosition (transform.position + movement);
        }


        void Turning ()
        {
#if !MOBILE_INPUT
            // crear un rayo en la posicion del mouse en el rango de la pantalla
            Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

            // crear un Raycashit para almacenar lo que alcanza el rayo
            RaycastHit floorHit;

            // ejecutar el Raycast si golpea algo del suelo.
            if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
            {
                // crear un vector para el mouse del suelo y la posicion
                Vector3 playerToMouse = floorHit.point - transform.position;

                // asegurar que el vector cubre la totalidad del suelo
                playerToMouse.y = 0f;

                // crea un Quaternion para la rotacion hecha por el mouse.
                Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

                // rotar al jugador hecha el nuevo giro.
                playerRigidbody.MoveRotation (newRotatation);
            }
#else

            Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotatation);
            }
#endif
        }


        void Animating (float h, float v)
        {
            // el booleano es verdad si los ejes son distintos de cero
            bool walking = h != 0f || v != 0f;

            // decir que el jugador esta caminando.
            anim.SetBool ("IsWalking", walking);
        }
    }
}