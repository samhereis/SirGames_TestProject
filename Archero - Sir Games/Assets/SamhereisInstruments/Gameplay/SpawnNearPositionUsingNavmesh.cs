using UnityEngine;
using UnityEngine.AI;

namespace Helpers
{
    public class SpawnNearPositionUsingNavmesh
    {
        public static Vector3 GetNearPosition(Vector3 position, float minRadius, float maxRadius)
        {
            float radius = Random.Range(minRadius, maxRadius);

            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += position;

            NavMesh.SamplePosition(randomDirection, out var hit, radius, 1);
            Vector3 finalPosition = hit.position;

            return finalPosition;
        }
    }
}