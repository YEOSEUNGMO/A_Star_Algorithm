using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace A_Star_Algorithm
{
    class Position
    {
        public Position(int _y,int _x) { y = _y;x = _x; }
        public int y;
        public int x;
    }
    class Player
    {
        public int y { get; private set; }
        public int x { get; private set; }

        Board board;

        Random rand = new Random();

        enum Direction
        {
            Up=0,
            Left,
            Down,
            Right

        }
        int direction = (int)Direction.Up;

        List<Position> positions = new List<Position>();

        public void Init(int _y, int _x, Board _board)
        {
            y = _y;
            x = _x;
            board = _board;

            AStar();
        }
        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo( PQNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1;
            }
        }
        public void AStar()
        {
            int[] delta_y = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };
            int[] delta_x = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };
            // 점수
            // F = G + H
            // F = 최종 점수(작을 수록 좋음, 경로에따라 달라짐)
            // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 ( 작을 수록 좋음, 경로에따라 달라짐)
            // H = 목적지에서 얼마나 가까운지 (작을 수록 좋음, 고정값)

            //(y,x) 방문 여부
            bool[,] closed = new bool[board.size, board.size];

            //(y,x) 가는 길을 한 번이라도 발견했는지 여부.
            // 발견 X -> MaxValue
            // 발견 O -> F = G + H
            int[,] open = new int[board.size, board.size];

            for (int y = 0; y < board.size; y++)
                for (int x = 0; x < board.size; x++)
                    open[y, x] = Int32.MaxValue;

            Position[,] parent = new Position[board.size, board.size];

            // 오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();


            //시작점 발견 (예약 진행)
            //F = G + H
            int g = 0;
            int h = 10 * (Math.Abs(board.DestY - y) + Math.Abs(board.DestX - x));
            int f = g + h;
            open[y, x] = f;
            pq.Push(new PQNode() { F = f, G = g, Y = y, X = x });
            parent[y, x] = new Position(y, x);
            while(pq.Count>0)
            {
                PQNode node = pq.Pop();
                if (closed[node.Y, node.X])
                    continue;

                closed[node.Y, node.X] = true;

                if (node.Y == board.DestY && node.X == board.DestX)
                    break;

                for (int i = 0; i < delta_y.Length; i++)
                {
                    int next_y = node.Y + delta_y[i];
                    int next_x = node.X + delta_x[i];
                    if (next_y < 0 || next_y >= board.size || next_x < 0 || next_x >= board.size)
                        continue;
                    if (board.tile[next_y, next_x] == Board.TileType.WALL)
                        continue;
                    if (closed[next_y, next_x] == true)
                        continue;

                    g = node.G + cost[i];
                    h = 10 * (Math.Abs(board.DestY - next_y) + Math.Abs(board.DestX - next_x));
                    f = g + h;
                    if (open[next_y, next_x] < f)
                        continue;
                    open[next_y, next_x] = f;

                    pq.Push(new PQNode() { F = f, G = g, Y = next_y, X = next_x });
                    parent[next_y, next_x] = new Position(node.Y, node.X);

                }
            }
            CalculateFromParent(parent);
        }
        public void BFS()
        {
            int[] delta_y = new int[] { -1, 0, 1, 0 };
            int[] delta_x = new int[] { 0, -1, 0, 1 };
            bool[,] visited = new bool[board.size, board.size];
            Position[,] parent = new Position[board.size, board.size];
            Queue<Position> q = new Queue<Position>();
            q.Enqueue(new Position(y, x));
            visited[y, x] = true;
            parent[y, x] = new Position(y, x);

            while(q.Count>0)
            {
                Position pos = q.Dequeue();
                int current_y = pos.y;
                int current_x = pos.x;

                for(int i=0;i<4;i++)
                {
                    int next_y = current_y + delta_y[i];
                    int next_x = current_x + delta_x[i];
                    if (next_y < 0 || next_y >= board.size || next_x < 0 || next_x >= board.size)
                        continue;
                    if (board.tile[next_y, next_x] == Board.TileType.WALL)
                        continue;
                    if (visited[next_y, next_x] == true)
                        continue;
                    q.Enqueue(new Position(next_y, next_x));
                    visited[next_y, next_x] = true;
                    parent[next_y, next_x] = new Position(current_y, current_x);
                }
            }
            CalculateFromParent(parent);
        }

        public void CalculateFromParent(Position[,] parent)
        {
            int dest_y = board.DestY;
            int dest_x = board.DestX;
            //while (parent[dest_y, dest_x].y != dest_y || parent[dest_y, dest_x].x != dest_x)
            while(true)
            {
                //시작점에서 break.
                if (parent[dest_y, dest_x].y == dest_y && parent[dest_y, dest_x].x == dest_x)
                    break;
                positions.Add(new Position(dest_y, dest_x));
                Position pos = parent[dest_y, dest_x];
                dest_y = pos.y;
                dest_x = pos.x;
            }

            positions.Add(new Position(dest_y, dest_x));
            positions.Reverse();
        }

        public void RightHand()
        {
            int[] MoveFrontY = new int[] { -1, 0, 1, 0 };
            int[] MoveFrontX = new int[] { 0, -1, 0, 1 };
            int[] MoveRightY = new int[] { 0, -1, 0, 1 };
            int[] MoveRightX = new int[] { 1, 0, -1, 0 };

            positions.Add(new Position(y, x));

            while (y!=board.DestY || x!=board.DestX)
            {
                //1. 현재 바라보는 방향을 기준으로 오른쪽으로 갈 수 있는지 확인.
                if(board.tile[y + MoveRightY[direction], x + MoveRightX[direction]] == Board.TileType.EMPTY)
                {
                    //오른쪽 방향으로 90도 회전.
                    direction = (direction - 1 + 4) % 4;
                    //앞으로 한보 전진.
                    y += MoveFrontY[direction];
                    x += MoveFrontX[direction];

                    positions.Add(new Position(y, x));
                }
                //2. 현재 바라보는 방향을 기준으로 전진할 수 있는지 확인.
                else if(board.tile[y+MoveFrontY[direction],x+MoveFrontX[direction]]==Board.TileType.EMPTY)
                {
                    //앞으로 한보 전진.
                    y += MoveFrontY[direction];
                    x += MoveFrontX[direction];

                    positions.Add(new Position(y, x));
                }
                else
                {
                    //왼쪽 방향으로 90도 회전
                    direction = (direction + 1 + 4) % 4;
                }
            }
        }

        const int MOVE_TIME = 100;
        int totalTime = 0;
        int lastIndex = 0;
        public void Update(int deltaTime)
        {
            if (lastIndex >= positions.Count)
            {
                lastIndex = 0;
                positions.Clear();
                board.Init(board.size, this);
                Init(1, 1, board);
            }
            totalTime += deltaTime;
            if(totalTime>=MOVE_TIME)
            {
                totalTime = 0;

                y = positions[lastIndex].y;
                x = positions[lastIndex].x;
                lastIndex++;

            }
        }
    }
}
