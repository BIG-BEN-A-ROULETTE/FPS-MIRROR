using UnityEngine;

[RequireComponent(typeof(PlayerMotor))] //veut dire que PlayerControler marche pas sans PlayerMotor.
[RequireComponent(typeof(ConfigurableJoint))]

public class PlayerControler : MonoBehaviour
{
    [SerializeField]    //pour faire aparaitre le private dans unity (inspector) et pour y acceder.
    private float speed = 7f;

    [SerializeField]    //pour faire aparaitre le private dans unity (inspector) et pour y acceder.
    private float mouseSensitivityX = 9f;

    [SerializeField]    //pour faire aparaitre le private dans unity (inspector) et pour y acceder.
    private float mouseSensitivityY = 10f;

    [SerializeField]
    private float thrusterForce = 1000f; //pour la pousser du jetpack.

    [Header("Joint Options")]   //optionel mais rajoute des titre dans l'inspector.
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor; //references
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();    //motor va stocker le script de Player motor et mtn on y a acces au script PLayerMotor depuis ici.
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        //Calculer la velociter des mouvement de notre joueurs.

        float xMov = Input.GetAxisRaw("Horizontal");    //configurer de base pour un clavier qwerty donc changement a faire directement dans Unity.
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;    //on transfome les input en valeur sur les axes(on precise les axes pour Unity).
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;  //on regrouppe et recuêre la velocite du joueur.

        motor.Move(velocity);   //on apllique velocity a notre personnage.


        //Calcul de la rotation du joueur en un vector3.

        float yRot = Input.GetAxisRaw("Mouse X");   //on a recuperer le input
        
        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;  //on transforme notre input en vector3 et on bloqueles axes x et z.

        motor.Rotate(rotation);


        //Calcul de la rotation de la camera en un vector3.

        float xRot = Input.GetAxisRaw("Mouse Y");   //on a recuperer le input

        float cameraRotationX = xRot * mouseSensitivityY;  //on transforme notre input en vector3 et on bloqueles axes y et z.

        motor.RotateCamera(cameraRotationX);

        //Calcul de la force du thruster.
        Vector3 thrusterVelocity = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            thrusterVelocity = Vector3.up * thrusterForce;
            SetJointSettings(0f);   //le personnage n'est pas tire vers le bas.
        }
        else
        {
            SetJointSettings(jointSpring);  //quand on a plus le jet-pack on est attirer vers le bas.
        }

        motor.ApplyThruster(thrusterVelocity);
    }


    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
