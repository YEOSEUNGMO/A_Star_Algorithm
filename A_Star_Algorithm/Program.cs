using System;

namespace A_Star_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();          //캐릭터가 움직일 맵 생성.
            Player player = new Player();       //캐릭터 생성.
            board.Init(25,player);              //맵의 크기와 캐릭터 정보로 맵 초기화.
            
            player.Init(1, 1, board);           //시작위치와 맵 정보로 캐릭터 초기화.
            Console.CursorVisible = false;

            const int WAIT_TICK = 1000 / 30;   //1초 동안 30프레임 출력.
            int lastTick = 0;
            while(true)
            {
                #region FrameManager
                int currentTick = System.Environment.TickCount & Int32.MaxValue;
                if (currentTick - lastTick < WAIT_TICK)
                {
                    continue;
                }
                int deltaTime = currentTick - lastTick;
                lastTick = currentTick;

                #endregion
                player.Update(deltaTime);       //캐릭터 정보 업데이트

                board.Render();                 //매 프레임마다 맵 정보 업데이트

                Console.SetCursorPosition(0, 0);//콘솔 커서 위치 (0,0) 으로 초기화
            }
        }
    }
}
