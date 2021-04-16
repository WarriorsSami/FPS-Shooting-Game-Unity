using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerMotor))] 
    [RequireComponent(typeof(ConfigurableJoint))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float lookSensitivity = 3f;
        
        [SerializeField] private float thrusterForce = 1400f;

        [Header("Joint options:")] 
        [SerializeField] private JointDriveMode jointMode = JointDriveMode.Position;
        [SerializeField] private float jointSpring = 15f;
        [SerializeField] private float jointMaxForce = 40f;
        
        private PlayerMotor _motor;
        private ConfigurableJoint _joint;
    
        private void Start()
        {
            _motor = GetComponent<PlayerMotor>();
            _joint = GetComponent<ConfigurableJoint>();
            
            SetJointSettings(jointSpring);
        }

        private void Update()
        {
            // calculate player velocity as a 3D vector
            var xMov = Input.GetAxisRaw("Horizontal");
            var zMov = Input.GetAxisRaw("Vertical");

            var transform1 = transform;
            var movHorizontal = transform1.right * xMov;
            var movVertical = transform1.forward * zMov;
        
            // final movement vector
            var velocity = (movHorizontal + movVertical).normalized * speed;
        
            // apply movement
            _motor.Move(velocity);
            
            // calculate player rotation as a 3D vector (turning around)
            var yRot = Input.GetAxisRaw("Mouse X");

            var rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
            
            // apply rotation
            _motor.Rotate(rotation);
            
            // calculate camera rotation as a 3D vector
            var xRot = Input.GetAxisRaw("Mouse Y");

            var cameraRotationX = xRot * lookSensitivity;
            
            // apply camera rotation
            _motor.RotateCamera(cameraRotationX);
            
            // calculate the thruster force
            var thrusterFr = Vector3.zero;
            if (Input.GetButton("Jump"))
            {
                thrusterFr = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
            else
            {
                SetJointSettings(jointSpring);
            }
            
            // apply the thruster force
            _motor.Push(thrusterFr);
        }

        private void SetJointSettings(float jointSpr)
        {
            _joint.yDrive = new JointDrive
            {
                mode = jointMode,
                positionSpring = jointSpr,
                maximumForce = jointMaxForce
            };
        }
    }
}
