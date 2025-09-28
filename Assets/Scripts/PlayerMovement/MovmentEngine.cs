using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEditor.Callbacks;


namespace KinematicCharacterControler
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MovementEngine : MonoBehaviour
    {
        public CapsuleCollider capsule;
        public float capsuleHeight = 2;
        public float capsuleRadius = 0.5f;
        public LayerMask collisionLayers;

        [Header("Collision & Slope")]
        public float skinWidth = 0.015f;
        public int maxBounces = 5;
        public float maxSlopeAngle = 55f;
        private float m_anglePower = 0.5f;

        [Header("Ground Checks")]
        public float defaultGroundCheck = 0.25f;
        public float defaultGroundedDistance = 0.05f;
        public float snapDownDistance = 0.45f; 
        public bool shouldSnapDown = true; // Should snap to ground
        private Vector3 prevPos;
        public Quaternion playerRotation;
       


        public GroundedState groundedState { get; protected set; }

        private Bounds bounds;
        public bool stuck = false;
        public Rigidbody rb;
        void Awake()
        {

            rb = GetComponent<Rigidbody>();
        }

        public Vector3 MovePlayer(Vector3 movement)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 remaining = movement;
            int bounces = 0;
            bool wasStuck = stuck;

            while (bounces < maxBounces && remaining.magnitude > 0.001f)
            {
                float distance = remaining.magnitude;
                if (!CastSelf(position, rotation, remaining.normalized, distance, out RaycastHit hit))
                {
                    position += remaining;
                    break;
                }

                // Improved stuck detection
                if (hit.distance < skinWidth * 2f)
                {
                    stuck = true;

                    // Try to resolve overlap by moving along hit normal
                    Vector3 pushOut = hit.normal * (skinWidth * 3f);
                    position += pushOut;

                    // Reduce remaining movement significantly when stuck
                    remaining *= 0.1f;

                    // If we're still stuck after a few attempts, try unstuck logic
                    if (bounces > 2)
                    {
                        TryUnstuck();
                        break;
                    }
                }
                else
                {
                    stuck = false;
                }

                float fraction = Mathf.Max(hit.distance - skinWidth, 0f) / distance;

                position += remaining * fraction;
                position += hit.normal * skinWidth; // Consistent skin width application
                remaining *= (1 - fraction);

                Vector3 planeNormal = hit.normal;
                float angleBetween = Vector3.Angle(hit.normal, remaining) - 90.0f;
                angleBetween = Mathf.Abs(angleBetween);
                float normalizedAngle = angleBetween / 90;

                remaining *= Mathf.Pow(1 - normalizedAngle, m_anglePower) * 0.9f + 0.1f;

                Vector3 projected = Vector3.ProjectOnPlane(remaining, planeNormal).normalized * remaining.magnitude;

                if (projected.magnitude + 0.001f < remaining.magnitude)
                {
                    remaining = Vector3.ProjectOnPlane(remaining, Vector3.up).normalized * remaining.magnitude;
                }
                else
                {
                    remaining = projected;
                }

                bounces++;
            }

            // Improved stuck handling - don't toggle rigidbody kinematic
            if (movement.magnitude > 0 && Vector3.Distance(prevPos, position) < 0.001f)
            {
                // Player is stuck, but don't change rigidbody state
                // Instead, try to resolve the stuck state
                if (!wasStuck) // Only try once per stuck event
                {
                    TryUnstuck();
                }
                // Still return the position we calculated
            }

            prevPos = position;
            return position;
        }


    public void TryUnstuck()
    {
        Vector3 originalPosition = transform.position;
        bool foundEscape = false;

        // Try different directions to escape
        Vector3[] escapeDirections = {
            Vector3.up * 0.3f,          // Try up first (most common case)
            -transform.right * 0.2f,     // Left
            transform.right * 0.2f,      // Right  
            transform.forward * 0.2f,    // Forward
            -transform.forward * 0.2f,   // Back
            -Vector3.up * 0.1f          // Slightly down (last resort)
        };

        foreach (Vector3 direction in escapeDirections)
        {
            Vector3 testPosition = originalPosition + direction;

            // Check if this position is clear
            if (!CastSelf(testPosition, transform.rotation, Vector3.zero, 0.01f, out RaycastHit hit))
            {
                transform.position = testPosition;
                foundEscape = true;
                Debug.Log($"Unstuck by moving {direction}");
                break;
            }
        }

        if (!foundEscape)
        {
            Debug.LogWarning("Could not find escape route from stuck position");
            // As last resort, move up more aggressively
            transform.position = originalPosition + Vector3.up * 0.5f;
        }
    }
    

        public bool CheckIfGrounded(out RaycastHit _hit)
        {


            if (CastSelf(transform.position, transform.rotation, Vector3.down, defaultGroundCheck, out _hit))
            {
                float angle = Vector3.Angle(_hit.normal, Vector3.up);
                bool isGrounded = _hit.distance <= (capsule.height / 2f) + defaultGroundedDistance;

                groundedState = new GroundedState(_hit.distance, isGrounded, angle, _hit.normal, _hit.point);
                return isGrounded;

            }
            else
            {
                groundedState = new GroundedState(defaultGroundCheck, false, 0f, Vector3.up, Vector3.zero);
                return false;
            }
        }



        public bool CastSelf(Vector3 pos, Quaternion rot, Vector3 dir, float dist, out RaycastHit hit)
        {
            Vector3 center = rot * capsule.center + pos;
            float radius = capsule.radius;
            float height = capsule.height;
        
            // Ensure we have valid dimensions
            Vector3 bottom = center + rot * Vector3.down * Mathf.Max(0.001f, (height / 2 - radius));
            Vector3 top = center + rot * Vector3.up * Mathf.Max(0.001f, (height / 2 - radius));
        
            // Use a more conservative skin width for casting
            float castRadius = Mathf.Max(0.001f, radius - skinWidth);
            
            RaycastHit[] hits = Physics.CapsuleCastAll(top, bottom, castRadius, dir, dist, collisionLayers, QueryTriggerInteraction.Ignore);
            
            // Filter out self collisions and invalid hits
            var validHits = hits.Where(h => h.collider != null && 
                                           h.collider.gameObject != gameObject &&
                                           h.distance >= 0f).ToArray();
            
            bool didHit = validHits.Length > 0;
        
            if (didHit)
            {
                // Find the closest valid hit
                hit = validHits.OrderBy(h => h.distance).First();
            }
            else
            {
                hit = new RaycastHit();
            }
        
            return didHit;
        }

        public void SnapPlayerDown()
        {
            bool closeToGround = CastSelf(
               transform.position,
               transform.rotation,
               Vector3.down,
               snapDownDistance,
               out RaycastHit groundHit);

            // If within the threshold distance of the ground
            if (closeToGround && groundHit.distance > 0)
            {
                // Snap the player down the distance they are from the ground
                transform.position += Vector3.down * (groundHit.distance - 0.001f);
            }
        }
       [System.Serializable]
        public struct GroundedState
        {
            public float distToGround;
            public bool isGrounded;
            public float angle;
            public Vector3 groundNormal;
            public Vector3 groundHitPosition;

            public GroundedState(float distToGround, bool isGrounded, float angle, Vector3 groundNormal, Vector3 groundHitPosition)
            {
                this.distToGround = distToGround;
                this.isGrounded = isGrounded;
                this.angle = angle;
                this.groundNormal = groundNormal;
                this.groundHitPosition = groundHitPosition;
            }

        }
    }
}
