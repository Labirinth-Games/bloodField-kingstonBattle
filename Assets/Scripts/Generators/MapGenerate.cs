using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Generators
{
    public class MapGenerate : MonoBehaviour
    {
        public List<Tile>[,] Build(int width, int height)
        {
            List<Tile>[,] grid = new List<Tile>[height, width];

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    grid[y, x] = new List<Tile>() { new Tile(y, x, TileTypeEnum.None) };
                }

            return grid;
        }
    }
}
