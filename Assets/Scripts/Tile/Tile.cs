using Cards;
using DG.Tweening;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Tile
    {
        public (int y, int x) position;
        public TileTypeEnum type;
        public GameObject gameObject;

        public Tile(int y, int x, TileTypeEnum tileType = TileTypeEnum.None, GameObject gameObject = null)
        {
            position.x = x;
            position.y = y;

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
        public void MoveTo((int y, int x) position)
        {
            //this.position = position;
            var x = position.x - this.position.x;
            var y = position.y - this.position.y;

            if (GameManager.Instance.mapManager.Register(this, (y, x)) == null)
                return;

            gameObject.transform.DOMove(GetPositionOnWorld(), .2f);
        }

        public Vector3 GetPositionOnWorld() => new Vector3(position.x, position.y, 0);
        public Vector3 SetPositionOnWorld() => gameObject.transform.position = GetPositionOnWorld();

        public bool IsEmpty() => type == TileTypeEnum.None;
        public bool AnyElement() => type != TileTypeEnum.None;

        private TileTypeEnum TranslateTypes(CardTypeEnum cardType)
        {
            switch (cardType)
            {
                case CardTypeEnum.Army: return TileTypeEnum.Army;
                case CardTypeEnum.Command: return TileTypeEnum.Command;
                case CardTypeEnum.Equipament: return TileTypeEnum.Equipament;
                case CardTypeEnum.Terrain: return TileTypeEnum.Terrain;
                default: return TileTypeEnum.None;
            }
        }
    }
}
