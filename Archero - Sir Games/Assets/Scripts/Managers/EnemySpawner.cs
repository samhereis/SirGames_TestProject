using Helpers;
using Identifiers;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EnemySpawner : MonoBehaviour, IInitializable
    {
        [SerializeField] private List<EnemyIdentifier> _enemiesToSpawn = new List<EnemyIdentifier>();

        [Header("Settings")]
        [SerializeField] private float _spawnRange = 10f;
        [SerializeField] private int _enemiesCountMin = 3;
        [SerializeField] private int _enemiesCountMax = 5;


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRange);
        }

        public void Initialize()
        {
            var enemiesCount = Random.Range(_enemiesCountMin, _enemiesCountMax);

            for (int i = 0; i < enemiesCount; i++)
            {
                var enemyPrefab = _enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Count)];
                var position = SpawnNearPositionUsingNavmesh.GetNearPosition(transform.position, 1, _spawnRange);

                var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                enemy.transform.SetParent(transform);
            }
        }
    }
}