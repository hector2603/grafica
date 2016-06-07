using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;            // The position that that camera will be following.
        public float smoothing = 5f;        // The speed with which the camera will be following.
		//Variables Necesarias para cambiar el tipo de vista
		public int zoomMaxPerspective= 60;    //El zoom máximo que puede tomar la cámara en vista de perspectiva
		public int zoomMinPerspective = 15;  //El zoom mínimo que puede tomar la cámara en vista de perspectiva
		public int zoomMaxOrthographic= 15;   //El zoom máximo que puede tomar la cámara en vista ortográfica
		public int zoomMinOrthographic = 5;   //El zoom mínimo que puede tomar la cámara en vista ortográfica
		//Estado de la cámara Ortografica inicializado en false,
		public bool orthoOn = false;   // ya que como predeterminada tendremos la de perspetiva
		public Camera MainCamera;  //Definimos el tipo de cámara principal (Perspectiva)
		public Camera Camera2;  //Definimos el tipo de cámara secundaria 
		public GameObject CamPerspective;   //Defiminos el tipo de Objeto de juego de la cámara en perspectiva
		public GameObject CamOrthographic;  //Defiminos el tipo de Objeto de juego de la cámara en ortográfica
		//Fin variables necesarias para cambiar el tipo de cámara
        Vector3 offset;                     // The initial offset from the target.


        void Start ()
        {
            // Calculate the initial offset.
            offset = transform.position - target.position;
			//Inicializamos los componentes al iniciar o ejecutar el juego
			CamPerspective = GameObject.Find("MainCamera");  //Traemos la Cámara del ambiente
			MainCamera = CamPerspective.GetComponent<Camera>(); //Asignamos los atributos de componente Cámara
			MainCamera.enabled = true;   //Habilitamos la cámara principal
			CamOrthographic = GameObject.Find ("Camera2");  //Traemos la Cámara secundaria del ambiente
			Camera2 = CamOrthographic.GetComponent<Camera>();  //Asignamos los atributos de componente Cámara
			Camera2.enabled = false;  //Inicializamos como desactivada la cámara secundaria
			// Fin objetos a inicializar
        }


		/* La función es la encargada de tener
		 * los oyentes para los eventos de teclado,
		 * o scroll del mouse para modificar el escalamiento
		 * de los diferentes escenarios con los diferentes
		 * tipos de vistas, también es el encargado de
		 * realizar el movimiento del jugador
		 */
        void FixedUpdate ()
        {
			if ( Input.GetKeyDown(KeyCode.Q) ){ //Si la fecha presionada es Q o q
				orthoOn = !orthoOn;             // la variable orthoOn cambia de estado
				if (orthoOn) {                // Si orthoOn es verdadero
					MainCamera.enabled = false;     //desactivamos la cámara en vista de perspectiva
					Camera2.enabled = true;		//activamos la cámara en vista de ortográfica	
				} else {
					MainCamera.enabled = true;  //activamos la cámara en vista de perspectiva
					Camera2.enabled = false;   //desactivamos la cámara en vista de ortográfica
				}
			}
			// Si el scroll del mouse es mayor a cero, y El actual Zoom de la cámara es mayor al zoom mínimo, y el estado de la cámara de perspectiva es true
			if((Input.GetAxis("Mouse ScrollWheel") > 0) && (MainCamera.fieldOfView > zoomMinPerspective) && (MainCamera.enabled == true)){
				MainCamera.fieldOfView = MainCamera.fieldOfView - 5; // Restamos el valor de 5 al zoom actual de la cámara
			}
			// Si el scroll del mouse es menor a cero, y El actual Zoom de la cámara es menor al zoom máximo, y el estado de la cámara de perspectiva es true
			if((Input.GetAxis ("Mouse ScrollWheel") < 0) && (MainCamera.fieldOfView < zoomMaxPerspective) && (MainCamera.enabled == true)) {
				MainCamera.fieldOfView = MainCamera.fieldOfView + 5;  // Sumamos el valor de 5 al zoom actual de la cámara
			}
			// Si el scroll del mouse es mayor a cero, y El actual Zoom de la cámara es mayor al zoom mínimo, y el estado de la cámara ortográfica es true
			if((Input.GetAxis("Mouse ScrollWheel") > 0) && (Camera2.orthographicSize > zoomMinOrthographic) && (Camera2.enabled == true)){
				Camera2.orthographicSize = Camera2.orthographicSize - 1; // Restamos el valor de 1 al zoom actual de la cámara
			}
			// Si el scroll del mouse es menor a cero, y El actual Zoom de la cámara es menor al zoom máximo, y el estado de la cámara ortográfica es true
			if((Input.GetAxis ("Mouse ScrollWheel") < 0) && (Camera2.orthographicSize < zoomMaxOrthographic) && (Camera2.enabled == true)) {
				Camera2.orthographicSize = Camera2.orthographicSize + 1;
			}
			//Sino cumple lo anterior, ejecute el movimiento del personaje
			else{
				// Create a postion the camera is aiming for based on the offset from the target.
				Vector3 targetCamPos = target.position + offset;
				// Smoothly interpolate between the camera's current position and it's target position.
				transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
			}
        }
		//Fin método FixedUpdate

    }
}