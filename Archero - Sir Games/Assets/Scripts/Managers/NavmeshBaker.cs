using Unity.AI.Navigation;
using UnityEngine;

namespace Managers
{
    public class NavmeshBaker : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface[] _navMeshSurfaces;

        private void Awake()
        {
            /*_navMeshSurfaces = FindObjectsOfType<NavMeshSurface>(true);

            foreach (NavMeshSurface navMeshSurface in _navMeshSurfaces)
            {
                navMeshSurface.BuildNavMesh();
            }*/
        }
    }
}