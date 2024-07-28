using Enums;
using Generators;
using Render;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace BloodField.Managers
{
    public class MapManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MapGenerate mapGenerate;
        [SerializeField] private List<Sprite> floorPrefabs;
        [SerializeField] private List<Sprite> baseSpawnSprites;

        [Header("Settings")]
        [SerializeField] private int spawnAreaScale;

        private List<Tile>[,] _map;
        private int _size;

        #region Gets/Sets
        public (int w, int h) Size() => (_size, _size);

        public Vector3 ReflexPosition(Vector3 pos) => new Vector3(pos.x, _size - pos.y, pos.y);
        public (int y, int x) ReflexPosition((int y, int x) pos) => (_size - pos.y - 1, pos.x);

        public List<Tile>[,] GetMap() => _map;

        public List<Tile> FindByPosition((int y, int x) position)
        {
            if (IsInsideMap(position))
                return _map[position.y, position.x];

            return null;
        }

        public (int y, int x) GetKingPositions()
        {
            var h_position = Mathf.FloorToInt(_size / 2);

            return (0, h_position);
        }

        public (int y, int x) GetKingEnemyPositions()
        {
            var h_position = Mathf.FloorToInt(_size / 2);

            return (_size - 1, h_position);
        }
        #endregion

        public void Load()
        {
            _size = GameManager.Instance.MatchSettings.MapSize;
            _map = mapGenerate.Build(_size, _size);

            MapRender.FloorRender(_map, floorPrefabs, baseSpawnSprites, spawnAreaScale);
        }

        #region Actions Map

        public Tile Register(Tile tile, (int y, int x) dir, bool ignoreObstacle = false)
        {
            // new position
            var newPosition = tile.NextPosition(dir);

            if (IsFreePositionMap(newPosition, ignoreObstacle))
            {
                // unregister last position
                Unregister(tile);

                // register new position
                tile.SetPosition(newPosition);
                _map[newPosition.y, newPosition.x].Add(tile);

                // if has tile empty joined with tile can remove it
                var tileEmpty = _map[tile.position.y, tile.position.x].Find(f => f.IsEmpty());

                if (tileEmpty is not null)
                {
                    _map[tile.position.y, tile.position.x].Remove(tileEmpty);
                }

                return tile;
            }

            return null;
        }

        public void Unregister(Tile tile)
        {
            if (_map[tile.position.y, tile.position.x].Exists(f => f.position == tile.position))
            {
                _map[tile.position.y, tile.position.x].Remove(tile);

                // idealmente sempre deixar um tile none para referencia de vazio
                if (_map[tile.position.y, tile.position.x].Count == 0)
                    _map[tile.position.y, tile.position.x].Add(new Tile(tile.position.y, tile.position.x, TileTypeEnum.None));
            }
        }

        public void Unregister(TileTypeEnum tileType, (int y, int x) position)
        {
            Tile tile = FindByPosition(position).Find(f => f.type == tileType);
            
            if(tile is null) return;

            if (_map[tile.position.y, tile.position.x].Exists(f => f.position == tile.position))
            {
                _map[tile.position.y, tile.position.x].Remove(tile);

                // idealmente sempre deixar um tile none para referencia de vazio
                if (_map[tile.position.y, tile.position.x].Count == 0)
                    _map[tile.position.y, tile.position.x].Add(new Tile(tile.position.y, tile.position.x, TileTypeEnum.None));
            }
        }

        #endregion

        #region Validatiors
        /**
         * valida se a posi��o passada por parametro est� dentro da grid
         * e est� vazia (sem nenhum elemento com parede, inimigos ou armadilhas)
         * **/
        public bool IsFreePositionMap((int y, int x) dir, bool ignoreObstacle = false)
        {
            if (IsInsideMap(dir))
                if (_map[dir.y, dir.x].Exists(f => f.CanMove() || ignoreObstacle))
                    return true;

            return false;
        }

        public bool CanSpawnMiniatures((int y, int x) dir)
        {
            if (IsInsideSpawnMap(dir))
                if (_map[dir.y, dir.x].Exists(f => f.CanMove()))
                    return true;

            return false;
        }

        public bool CanSpawnUntilMiddleMiniatures((int y, int x) dir)
        {
            if (IsInsideSpawnUntilMiddleMap(dir))
                if (_map[dir.y, dir.x].Exists(f => f.CanMove()))
                    return true;

            return false;
        }

        public bool IsInsideMap((int y, int x) dir) => dir.x >= 0 && dir.x < _size && dir.y >= 0 && dir.y < _size;
        public bool IsInsideSpawnMap((int y, int x) dir) => dir.x >= 0 && dir.x < _size && dir.y >= 0 && dir.y < spawnAreaScale;
        public bool IsInsideSpawnUntilMiddleMap((int y, int x) dir) => dir.x >= 0 && dir.x < _size && dir.y >= 0 && dir.y < _size - spawnAreaScale;
        #endregion

        private void OnValidate()
        {
            if (TryGetComponent(out MapGenerate mapGenerate))
                this.mapGenerate = mapGenerate;
        }
    }
}
