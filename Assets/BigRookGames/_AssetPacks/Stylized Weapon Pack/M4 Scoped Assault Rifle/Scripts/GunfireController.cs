using UnityEngine;

namespace BigRookGames.Weapons
{
    public class GunfireController : MonoBehaviour
    {
        // --- Audio ---
        public AudioClip GunShotClip;
        public AudioSource source;
        public Vector2 audioPitch = new Vector2(.9f, 1.1f);

        // --- Muzzle ---
        public GameObject muzzlePrefab;
        public GameObject muzzlePosition;

        // --- Config ---
        public bool autoFire;
        public float shotDelay = 0.5f;
        public bool rotate = true;
        public float rotationSpeed = 0.25f;

        // --- Options ---
        public GameObject scope;
        public bool scopeActive = true;
        private bool lastScopeState;

        // --- Projectile ---
        [Tooltip("The projectile GameObject to instantiate each time the weapon is fired.")]
        public GameObject projectilePrefab;
        [Tooltip("Mesh to disable when firing (e.g., visible rocket in a launcher).")]
        public GameObject projectileToDisableOnFire;

        // --- Timing ---
        [SerializeField] private float timeLastFired;

        private void Start()
        {
            if (source != null) source.clip = GunShotClip;
            timeLastFired = 0;
            lastScopeState = scopeActive;
        }

        private void Update()
        {
            // Rotate weapon if enabled
            if (rotate)
            {
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    transform.localEulerAngles.y + rotationSpeed,
                    transform.localEulerAngles.z
                );
            }

            // Fire weapon on auto-fire or left mouse click
            if ((autoFire || Input.GetMouseButton(0)) && (timeLastFired + shotDelay) <= Time.time)
            {
                FireWeapon();
            }

            // Toggle scope state
            if (scope && lastScopeState != scopeActive)
            {
                lastScopeState = scopeActive;
                scope.SetActive(scopeActive);
            }
        }

        /// <summary>
        /// Fires the weapon by creating a muzzle flash, playing sound, and spawning a projectile.
        /// </summary>
        private void FireWeapon()
        {
            timeLastFired = Time.time;

            // Spawn muzzle flash
            if (muzzlePrefab != null && muzzlePosition != null)
            {
                Instantiate(muzzlePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation);
            }

            // Spawn projectile
            if (projectilePrefab != null && muzzlePosition != null)
            {
                GameObject newProjectile = Instantiate(
                    projectilePrefab,
                    muzzlePosition.transform.position,
                    muzzlePosition.transform.rotation
                );

                Projectile projectileScript = newProjectile.GetComponent<Projectile>();

                if (projectileScript != null)
                {
                    Vector3 shootDirection = muzzlePosition.transform.forward; // Ensures forward motion
                    projectileScript.SetDirection(shootDirection);

                    Debug.Log("Firing Projectile in Direction: " + shootDirection);
                }
            }

            // Play gunshot audio
            if (source != null)
            {
                source.Play();
            }
        }



        private void ReEnableDisabledProjectile()
        {
            if (projectileToDisableOnFire != null)
            {
                projectileToDisableOnFire.SetActive(true);
            }
        }
    }
}
