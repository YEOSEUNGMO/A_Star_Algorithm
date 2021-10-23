using System;
using System.Collections.Generic;
using System.Text;

namespace A_Star_Algorithm
{
    class Board
    {
        const char CIRCLE = '\u25cf';

        public TileType[,] tile { get; private set; }
        public int size { get; private set; }

        public int DestX { get; private set; }
        public int DestY { get; private set; }

        Player player;
        public enum TileType
        {
            EMPTY,
            WALL
        }
        public void Init(int _size,Player _player)
        {
            if (_size % 2 == 0) return;
            tile = new TileType[_size, _size];
            size = _size;
            player = _player;
            DestY = size - 2;
            DestX = size - 2;
            //GenerateByBinaryTree();
            GenerateBySideWinder();
        }
        //사이드 와인더로 맵 생성하기.
        public void GenerateBySideWinder()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        tile[y, x] = TileType.WALL;
                    }
                    else
                    {
                        tile[y, x] = TileType.EMPTY;
                    }
                }
            }

            Random rand = new Random();
            for (int y = 0; y < size; y++)
            {
                int count =  1;
                for (int x = 0; x < size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (y == size - 2 && x == size - 2)
                        continue;
                    if (y == size - 2)
                    {
                        tile[y, x + 1] = TileType.EMPTY;
                        continue;
                    }
                    if (x == size - 2)
                    {
                        tile[y + 1, x] = TileType.EMPTY;
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        tile[y, x + 1] = TileType.EMPTY;
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        tile[y + 1, x - randomIndex * 2] = TileType.EMPTY;
                        count = 1;
                    }
                }
            }
        }

        //바이너리트리로 맵 생성하기.
        public void GenerateByBinaryTree()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        tile[y, x] = TileType.WALL;
                    }
                    else
                    {
                        tile[y, x] = TileType.EMPTY;
                    }
                }
            }

            Random rand = new Random();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;
                    if (y == size - 2 && x == size - 2)
                        continue;
                    if (y == size - 2)
                    {
                        tile[y, x + 1] = TileType.EMPTY;
                        continue;
                    }
                    if (x == size - 2)
                    {
                        tile[y + 1, x] = TileType.EMPTY;
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        tile[y, x + 1] = TileType.EMPTY;
                    }
                    else
                    {
                        tile[y + 1, x] = TileType.EMPTY;
                    }
                }
            }
        }

        public void Render()
        {
            ConsoleColor prvColor = Console.ForegroundColor;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if(y==player.y&&x==player.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if(y==DestY&&x==DestX)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = GetTileColor(tile[y, x]);
                    }                    
                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prvColor;
        }

        ConsoleColor GetTileColor(TileType type)
        {
            switch(type)
            {
                case TileType.EMPTY:
                    return ConsoleColor.Green;
                case TileType.WALL:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;
            }
        }
    }
}
