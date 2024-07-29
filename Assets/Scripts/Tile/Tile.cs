using BloodField.Types;
using DG.Tweening;
using BloodField.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;

namespace Tiles
{
    public class Tile
    {
        public (int y, int x) position;
        public TileType type;
        public GameObject gameObject;

        public Action<(int y, int x), (int y, int x)> OnTileMove;

        public Tile(int y, int x, TileType tileType = TileType.None, GameObject gameObject = null)
        {
            position.x = x;
            position.y = y;

            type = tileType;
            this.gameObject = gameObject;
        }

        public Tile((int y, int x) position, TileType tileType = TileType.None, GameObject gameObject = null)
        {
            this.position = position;

            type = tileType;
            this.gameObject = gameObject;
        }

        public Tile(TileType tileType = TileType.None, GameObject gameObject = null)
        {
            type = tileType;
            this.gameObject = gameObject;
        }

        public Tile(CardType tileType, GameObject gameObject = null)
        {
            type = TranslateTypes(tileType);
            this.gameObject = gameObject;
        }

        public (int y, int x) NextPosition((int y, int x) dir)
        {
            // new position
            int x = position.x + dir.x;
            int y = position.y + dir.y;

            return (y, x);
        }

        public void SetPosition((int y, int x) pos) => this.position = pos;

        // used to move aposition filtred on map
        public Vector3 MoveTo((int y, int x) position)
        {
            var lastPosition = this.position;
            var x = position.x - this.position.x;
            var y = position.y - this.position.y;

            var tile = GameManager.Instance.mapManager.Register(this, (y, x));

            if (tile is null)
                return Vector3.zero;

            if (OnTileMove != null)
                OnTileMove(lastPosition, tile.position);

            gameObject.transform.DOMove(GetPositionOnWorld(), .2f);

            return GetPositionOnWorld();
        }

        public void MoveBack(int value)
        {
            MoveTo((position.y - value, position.x));
        }

        public Vector3 GetPositionOnWorld() => new Vector3(position.x, position.y, 0);
        public Vector3 SetPositionOnWorld() => gameObject.transform.position = GetPositionOnWorld();

        public bool IsEmpty() => type == TileType.None;
        public bool IsTerrain() => type == TileType.Terrain;
        public bool IsArmy() => type == TileType.Army;
        public bool AnyElement() => type != TileType.None;
        public bool CanMove() => new TileType[] { TileType.None, TileType.Terrain }.Contains(type);
        public bool IsATarget() => new TileType[] { TileType.Army, TileType.Equipament, TileType.King }.Contains(type);

        private TileType TranslateTypes(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.Army: return TileType.Army;
                case CardType.Equipament: return TileType.Equipament;
                case CardType.Terrain: return TileType.Terrain;
                case CardType.King: return TileType.King;
                default: return TileType.None;
            }
        }
    }
}
