using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float delayToFollow = 0.2f;
    public float padding = 4f;
    public float minSize = 6.5f;
    [HideInInspector] public Transform[] targetsToFollow;


    private Camera mainCamera;
    private float zoomSpeed;
    private Vector3 moveSpeed;
    private Vector3 desiredPosition;


    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        //Mueve la posicion de la camara suavemente desde su posicion actual a la posicion deseada tomando como referencia la velocidad de movimiento y aplicando un delay que es lo que tardara en empezar a moverse y llegar a la posicion deseada
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveSpeed, delayToFollow);
    }

    //Esta funcion y su resultado usandose en Move() son la clave para que la camara se mueva como la de super smash bros
    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targetsToFollow.Length; i++)
        {
            if (!targetsToFollow[i].gameObject.activeSelf)
            {
                //continue indica que si este if no se cumple lo que sigue despues de el no se ejecutara, entonces el for seguira con la vuelta siguiente
                continue;
            }

            averagePos += targetsToFollow[i].position;
            numTargets++;
        }

        if (numTargets > 0)
        {
            averagePos /= numTargets;
        }

        //Solo se mueve en "y" porque de eso se encarga Move(), de mover en la camara solo en "y", y Zoom() en "x"
        averagePos.y = transform.position.y;

        desiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        
        //Cambia el tamaño ortografico de la camara suavemente desde su tamaño actual al tamaño deseado tomando como referencia la velocidad de zoom y aplicando un delay que es lo que tardara en empezar a cambiar de tamaño y llegar al tamaño requerido
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, requiredSize, ref zoomSpeed, delayToFollow);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        //Variable que guardara el nuevo tamaño de la camara (que aqui se usa como zoom), recordar que el tamaño se mide en diagonal, y el valor de esta variable al ser devuelto por la funcion se guardara en requiredSize en Zoom() para cambiar el tamaño ortografico de la camara
        float size = 0f;

        for (int i = 0; i < targetsToFollow.Length; i++)
        {
            if (!targetsToFollow[i].gameObject.activeSelf)
            {
                continue;
            }

            Vector3 targetLocalPos = transform.InverseTransformPoint(targetsToFollow[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            //Mathf.Max() elige el valor mas grande entre dos parametros
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            //mainCamera.aspect es el tamaño de la camara en diagonal
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / mainCamera.aspect);
        }

        size += padding;

        size = Mathf.Max(size, minSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = desiredPosition;

        mainCamera.orthographicSize = FindRequiredSize();
    }
}