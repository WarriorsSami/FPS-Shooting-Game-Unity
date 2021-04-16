using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField] private Behaviour[] componentsToDisable;
        private Camera _sceneCamera;
        
        [SerializeField] private string remoteLayerName = "RemotePlayer";
    
        private void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();
                AssignRemoteLayer();
            }
            else
            {
                _sceneCamera = Camera.main;
                if (_sceneCamera != null)
                {
                    _sceneCamera.gameObject.SetActive(false);
                }
            }
            
            RegisterPlayer();
        }

        private void RegisterPlayer()
        {
            var id = "Player " + GetComponent<NetworkIdentity>().netId;
            transform.name = id;
        }
        
        private void AssignRemoteLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        }

        private void DisableComponents()
        {
            foreach (var comp in componentsToDisable)
            {
                comp.enabled = false;
            }
        }

        private void OnDisable()
        {
            if (_sceneCamera != null)
            {
                _sceneCamera.gameObject.SetActive(true);
            }
        }
    }
}
