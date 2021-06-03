using UnityEngine;

[RequireComponent(typeof(Rigidbody))]   //pour que Player motor puisse fonctionner il lui faut rigidbody
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity;   // de meme nom que la valeur recuperer dans playerControler.
    private Vector3 rotation;
    private float cameraRotationX = 0f;
    private Vector3 thrusterVelocity;
    private float currentCameraRotationX = 0f;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity) //_pour differecier le parametre de la valeur generale.
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation) //_ pour differecier le parametre de la valeur generale, on fais pour gagner laccer depuis les variables definie plus haut.
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX) //_ pour differecier le parametre de la valeur generale.
    {
        cameraRotationX = _cameraRotationX;
    }

    public void ApplyThruster(Vector3 _thrusterVelocity) //_velocity pour differecier le parametre de la valeur generale.
    {
        thrusterVelocity = _thrusterVelocity;
    }

    private void FixedUpdate()  //pour effectuer en temps reel.
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);  //On aplique le mouvement au rb.
        }

        if(thrusterVelocity != Vector3.zero)
        {
            rb.AddForce(thrusterVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);    //On apllique le mouvenet du jet pack au rb.
        }
    }

    private void PerformRotation()
    {
        //Calcul de la rotation de la camera.
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));  //On applique la roattion au rb, pour transfo un vector3 en Quat on a fais = quat.eul('vector3').
        currentCameraRotationX -= cameraRotationX;  //- car on inverse les axes.
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
