using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable] public class PlayerWeapon : MonoBehaviour
    {
        public string weaponName = "Glock";
        public float damage = 10f;
        public float range = 100f;
    }
}
