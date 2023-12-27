using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts
{
    public class Tile : MonoBehaviour
    {
        public GameObject[] tilePrefabs;
        private List<GameObject> activeTiles = new List<GameObject>();

        private float _spawnPos = 0;
        private float tileLength = 200;

        [SerializeField] private Transform player;
        private int startTiles = 5;

        private void Start()
        {
            for (int i = 0; i < startTiles; i++)
            {
                if (i == 0)
                {
                    SpawnTile(0);
                }
                SpawnTile(Random.Range(0, tilePrefabs.Length));
            }
        }

        private void Update()
        {
            if (!(player.position.z - 120 > _spawnPos - (startTiles * tileLength))) return;
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }

        private void SpawnTile(int tileIndex)
        {
            var nextTile = Instantiate(tilePrefabs[tileIndex], transform.forward * _spawnPos, transform.rotation);
            activeTiles.Add(nextTile);
            _spawnPos += tileLength;
        }

        private void DeleteTile()
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
    }
}