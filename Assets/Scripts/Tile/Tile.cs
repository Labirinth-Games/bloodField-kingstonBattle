using Enums;
using DG.Tweening;
using Managers;
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
        public TileTypeEnum type;
        public GameObject gameObject;

        public Action<(int y, int x), (int y, int x)> OnTileMove;

        public Tile(int y, int x, TileTypeEnum tileType = TileTypeEnum.None, GameObject gameObject = null)
        {
            position.x = x;
            position.y = y;

            type = tileType;
            this.gameObject = gameObject;
        }

        public Tile((int y, int x) position, TileTypeEnum tileType = TileTypeEnum.None, GameObject gameObject = null)
        {
            this.position = position;

            type = tileType;
            this.gameObject = gameObject;
        }

        public Tile(TileTypeEnum tileType = TileTypeEnum.None, GameObject gameObject = null)
        {
            type = tileType;
            this.gameObject = gameObject;
        }

        public Tile(CardTypeEnum tileType, GameObject gameObject = null)
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
        public Vector3 MoveTo((int y, int x) position, bool withAnimation = true)
        {
            var lastPosition = this.position;
            var x = position.x - this.position.x;
            var y = position.y - this.position.y;

            var tile = GameManager.Instance.mapManager.Register(this, (y, x));

            if (tile is null)
                return Vector3.zero;

            if (OnTileMove != null)
                OnTileMove(lastPosition, tile.position);

            // if (withAnimation)
            // {
            //     //CmdMoveDirection();
            //     //gameObjectTile.transform.DOMove(GetPositionOnWorld(), .2f);
            // }
        // TODO - talvez possamos trazer para ca o move da miniatura pelomenos a chamada do gameobject ja que n conseguimos via rede chamar o command e clientRpc aqui
            return GetPositionOnWorld();
        }

        public Vector3 GetPositionOnWorld() => new Vector3(position.x, position.y, 0);
        public Vector3 SetPositionOnWorld() => gameObject.transform.position = GetPositionOnWorld();

        public bool IsEmpty() => type == TileTypeEnum.None;
        public bool IsTerrain() => type == TileTypeEnum.Terrain;
        public bool AnyElement() => type != TileTypeEnum.None;
        public bool CanMove() => new TileTypeEnum[] { TileTypeEnum.None, TileTypeEnum.Terrain }.Contains(type);
        public bool IsATarget() => new TileTypeEnum[] { TileTypeEnum.Army, TileTypeEnum.Equipament, TileTypeEnum.King }.Contains(type);

        private TileTypeEnum TranslateTypes(CardTypeEnum cardType)
        {
            switch (cardType)
            {
                case CardTypeEnum.Army: return TileTypeEnum.Army;
                case CardTypeEnum.Equipament: return TileTypeEnum.Equipament;
                case CardTypeEnum.Terrain: return TileTypeEnum.Terrain;
                case CardTypeEnum.King: return TileTypeEnum.King;
                default: return TileTypeEnum.None;
            }
        }
    }
}
