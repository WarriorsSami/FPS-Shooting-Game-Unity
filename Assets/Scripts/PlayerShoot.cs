using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class PlayerShoot : NetworkBehaviour
    {
        private const string PlayerTag = "Player";
        private const float Offset = 0.5f;
        public PlayerWeapon weapon;
        
        [SerializeField] private Camera cam;

        [SerializeField] private LayerMask mask;
        
        private void Start()
        {
            if (cam != null) return;
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        private void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
            }
        }

        [Client] private void Shoot()
        {
            RaycastHit hit;
            var footStepRay = new Ray(cam.transform.position + Vector3.back * Offset, Vector3.forward);            

            if (Physics.Raycast(footStepRay, out hit, weapon.range, mask))
            {
                // we hit something
                //if (hit.collider.CompareTag(PlayerTag))
                //{
                    CmdPlayerShot(hit.collider.name);
                //}
            }
        }

        [Command] private void CmdPlayerShot(string playerId)
        {
            Debug.Log(playerId + " has been shot!");
            //Destroy(GameObject.Find (id));
        }
    }
}
