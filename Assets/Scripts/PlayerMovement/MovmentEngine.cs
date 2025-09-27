using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;


namespace KinematicCharacterControler
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MovementEngine : MonoBehaviour
    {
          public CapsuleCollider capsule;
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


        public GroundedState groundedState { get; protected set; }

        private Bounds bounds;


        void Awake()
        {
            
        }

        public Vector3 MovePlayer(Vector3 movement)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            Vector3 remaining = movement;

            int bounces = 0;

            while (bounces < maxBounces && remaining.magnitude > 0.001f)
            {
                // Do a cast of the collider to see if an object is hit during this
                // movement bounce
                float distance = remaining.magnitude;
                if (!CastSelf(position, rotation, remaining.normalized, distance, out RaycastHit hit))
                {
                    // If there is no hit, move to desired position
                    position += remaining;

                    // Exit as we are done bouncing
                    break;
                }

                // If we are overlapping with something, just exit.
                if (hit.distance == 0)
                {
                    break;
                }

                float fraction = hit.distance / distance;

                // Set the fraction of remaining movement (minus some small value)
                position += remaining * (fraction);
                // Push slightly along normal to stop from getting caught in walls
                position += hit.normal * 0.001f * 2;
                // Decrease remaining movement by fraction of movement remaining
                remaining *= (1 - fraction);

                // Plane to project rest of movement onto
                Vector3 planeNormal = hit.normal;

                // Only apply angular change if hitting something
                // Get angle between surface normal and remaining movement
                float angleBetween = Vector3.Angle(hit.normal, remaining) - 90.0f;

                // Normalize angle between to be between 0 and 1
                // 0 means no angle, 1 means 90 degree angle
                angleBetween = Mathf.Min(60f, Mathf.Abs(angleBetween));
                float normalizedAngle = angleBetween / 60f;

                // Reduce the remaining movement by the remaining movement that ocurred
                remaining *= Mathf.Pow(1 - normalizedAngle, m_anglePower) * 0.9f + 0.1f;

                // Rotate the remaining movement to be projected along the plane of the hit surface
                Vector3 projected = Vector3.ProjectOnPlane(remaining, planeNormal).normalized * remaining.magnitude;

                // If projected remaining movement is less than original remaining movement (broke from floating point),
                // then change this to just project along the vertical.
                if (projected.magnitude + 0.001f < remaining.magnitude)
                {
                    remaining = Vector3.ProjectOnPlane(remaining, Vector3.up).normalized * remaining.magnitude;
                }
                else
                {
                    remaining = projected;
                }

                // Track number of times the character has bounced
                bounces++;
            }

             
            return position;
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

            // Get top and bottom points of collider
            Vector3 bottom = center + rot * Vector3.down * (height / 2 - radius);
            Vector3 top = center + rot * Vector3.up * (height / 2 - radius);

            IEnumerable<RaycastHit> hits = Physics.CapsuleCastAll( top, bottom, radius, dir, dist, collisionLayers, QueryTriggerInteraction.Ignore);
            bool didHit = hits.Count() > 0;

            // Find the closest objects hit
            float closestDist = didHit ? Enumerable.Min(hits.Select(hit => hit.distance)) : 0;
            IEnumerable<RaycastHit> closestHit = hits.Where(hit => hit.distance == closestDist);

            hit = closestHit.FirstOrDefault();

            // Return if any objects were hit
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
