using Managers;
using System;
using System.Collections.Generic;
using Enums;
using Tiles;
using UnityEngine;


namespace Helpers
{
    public class ScanHelper : MonoBehaviour
    {
        public static List<Tile> Scan(Tile tile, ScanDirectionTypeEnum scanDirectionType = ScanDirectionTypeEnum.Ring, float amplitude = 1, bool overcomeObstacles = false)
        {
            List<Tile> tileList = new List<Tile>();

            if(amplitude < 1) return tileList;

            // positions around tile
            (int y, int x)[] positions = GetPositionByScanDirection(scanDirectionType, tile, amplitude, overcomeObstacles);
            List<Tile>[,] map = GameManager.Instance.mapManager.GetMap();

            foreach (var position in positions)
                tileList.AddRange(map[position.y, position.x]);

            return tileList;
        }

        public static List<(int y, int x)> ScanFixed(Tile tile, int width, int height, bool overcomeObstacles = false)
        {
            List<(int y, int x)> tileList = new List<(int y, int x)>();

            // positions around tile
            (int y, int x)[] positions = ScanDirectionFixed(tile, width, height, overcomeObstacles);
            List<Tile>[,] map = GameManager.Instance.mapManager.GetMap();

            foreach (var position in positions)
                tileList.Add(position);

            return tileList;
        }

        /**
         * Esse metodo vai tentar escaniar o quadrante baseado no scan direciton type,
         * basicamente vai pegar todas as celulas envolta do tile recebido e baseado na amplitude
         * ver quantos niveis ele vai buscar ou seja se tentar escaniar uma amplitude 2 ele vai
         * pegar como no exemplo abaixo:
         * 
         *      x x x                  x x x x x
         *      x T x <-- amplitude 1  x x x x x
         *      x x x                  x x T x x <-- amplitude 2
         *                             x x x x x
         *                             x x x x x
         *                             
         * com base nessa info ele vai buscar quem ele quer encontrar dentro da area.
         * **/
        private static (int y, int x)[] GetPositionByScanDirection(ScanDirectionTypeEnum scanDirectionType, Tile tile, float amplitude, bool overcomeObstacles)
        {
            Dictionary<ScanDirectionTypeEnum, Func<Tile, float, bool, (int y, int x)[]>> ScanStrategy = new Dictionary<ScanDirectionTypeEnum, Func<Tile, float, bool, (int y, int x)[]>>()
            {
                { ScanDirectionTypeEnum.Cross,              ScanCrossDirection},
                { ScanDirectionTypeEnum.Ring,               ScanRingDirection},
                { ScanDirectionTypeEnum.Horizontal_Line,    ScanHorizontalLineDirection},
                { ScanDirectionTypeEnum.Vertical_Line,      ScanVerticalLineDirection}
            };

            return ScanStrategy[scanDirectionType](tile, amplitude, overcomeObstacles);
        }

        #region Type Scans
        public static (int y, int x)[] ScanRingDirection(Tile tile, float amplitude, bool overcomeObstacles)
        {
            List<(int y, int x)> positions = new List<(int y, int x)>();
            bool[] canContinue = new bool[8] { true, true, true, true, true, true, true, true };

            for (int i = 1; i <= amplitude; i++)
            {
                var scanDirection = new (int y, int x, int id)[] {
                            (tile.position.y, tile.position.x + i, 0), // right
                            (tile.position.y + i * -1, tile.position.x + i, 1), // right - down
                            (tile.position.y + i * -1, tile.position.x, 1), // bottom
                            (tile.position.y + i * -1, tile.position.x + i * -1, 1), // left - bottom
                            (tile.position.y, tile.position.x + i * -1, 1), // left
                            (tile.position.y + i, tile.position.x + i * -1, 1), // left - up
                            (tile.position.y + i, tile.position.x, 1), // up
                            (tile.position.y + i, tile.position.x + i, 1), // up - right
                };

                foreach (var direction in scanDirection)
                {
                    if (canContinue[direction.id] && GameManager.Instance.mapManager.IsInsideMap((direction.y, direction.x)))
                    {
                        var tileFinded = GameManager.Instance.mapManager.FindByPosition((direction.y, direction.x));

                        if (tileFinded.Exists(f => f.CanMove()) || overcomeObstacles)
                            positions.Add((direction.y, direction.x)); // up
                        else
                            canContinue[direction.id] = false;
                    }
                }
            }

            return positions.ToArray();
        }

        public static (int y, int x)[] ScanCrossDirection(Tile tile, float amplitude, bool overcomeObstacles)
        {
            List<(int y, int x)> positions = new List<(int y, int x)>();
            bool[] canContinue = new bool[4] { true, true, true, true }; // amount direction configured below

            for (int i = 1; i <= amplitude; i++)
            {
                var scanDirection = new (int x, int y, int id)[] {
                            (tile.position.x + i, tile.position.y, 0), // right
                            (tile.position.x + i * -1, tile.position.y, 1), // left
                            (tile.position.x, tile.position.y + i, 2), // up
                            (tile.position.x, tile.position.y + i * -1, 3), // down
                };

                foreach (var direction in scanDirection)
                {
                    if (canContinue[direction.id] && GameManager.Instance.mapManager.IsInsideMap((direction.y, direction.x)))
                    {
                        var tileFinded = GameManager.Instance.mapManager.FindByPosition((direction.y, direction.x));

                        if (tileFinded.Exists(f => f.CanMove()) || overcomeObstacles)
                            positions.Add((direction.y, direction.x)); // up
                        else
                            canContinue[direction.id] = false;
                    }
                    else
                        canContinue[direction.id] = false;
                }
            }

            return positions.ToArray();
        }

        public static (int y, int x)[] ScanHorizontalLineDirection(Tile tile, float amplitude, bool overcomeObstacles)
        {
            List<(int y, int x)> positions = new List<(int y, int x)>();
            bool[] canContinue = new bool[2] { true, true }; // amount direction configured below

            for (int i = 1; i <= amplitude; i++)
            {
                var scanDirection = new (int x, int y, int id)[] {
                            (tile.position.x + i, tile.position.y, 0), // right
                            (tile.position.x + i * -1, tile.position.y, 1) // left
                };

                foreach (var direction in scanDirection)
                {
                    if (canContinue[direction.id] && GameManager.Instance.mapManager.IsInsideMap((direction.y, direction.x)))
                    {
                        var tileFinded = GameManager.Instance.mapManager.FindByPosition((direction.y, direction.x));
                        if (tileFinded.Exists(f => f.CanMove()) || overcomeObstacles)
                            positions.Add((direction.y, direction.x)); // up
                        else
                            canContinue[direction.id] = false;
                    }
                    else
                        canContinue[direction.id] = false;
                }
            }

            return positions.ToArray();
        }

        public static (int y, int x)[] ScanVerticalLineDirection(Tile tile, float amplitude, bool overcomeObstacles)
        {
            List<(int y, int x)> positions = new List<(int y, int x)>();
            bool[] canContinue = new bool[2] { true, true }; // amount direction configured below

            for (int i = 1; i <= amplitude; i++)
            {
                var scanDirection = new (int x, int y, int id)[]
                {
                    (tile.position.x, tile.position.y + i, 0), // up
                    (tile.position.x , tile.position.y + i * -1, 1) // down
                };

                foreach (var direction in scanDirection)
                {
                    if (canContinue[direction.id] && GameManager.Instance.mapManager.IsInsideMap((direction.y, direction.x)))
                    {
                        var tileFinded = GameManager.Instance.mapManager.FindByPosition((direction.y, direction.x));

                        if (tileFinded.Exists(f => f.CanMove()) || overcomeObstacles)
                            positions.Add((direction.y, direction.x));
                        else
                            canContinue[direction.id] = false;
                    }
                    else
                        canContinue[direction.id] = false;
                }
            }

            return positions.ToArray();
        }

        public static (int y, int x)[] ScanDirectionFixed(Tile tile, int width, int height, bool overcomeObstacles)
        {
            List<(int y, int x)> positions = new List<(int y, int x)>();

            for (var y = tile.position.y - height + 1; y <= tile.position.y; y++)
                for (var x = tile.position.x; x < tile.position.x + width; x++)
                {
                    if (GameManager.Instance.mapManager.IsInsideMap((y, x)))
                    {
                        var tileFinded = GameManager.Instance.mapManager.FindByPosition((y, x));

                        if (tileFinded.Exists(f => f.IsEmpty()) || overcomeObstacles)
                            positions.Add((y, x));
                    }
                }

            return positions.ToArray();
        }
        #endregion

        #region Validators
        public static Tile CanMoveToTile(List<Tile> tiles, (int y, int x) position) => tiles.Find(e => e.position == position && e.CanMove());
        public static Tile CanAttackTile(List<Tile> tiles, (int y, int x) position) => tiles.Find(e => e.position == position && e.IsATarget());
        #endregion
    }
}
