using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    class GameEngine
    {   
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Ticks { get; set; }
        private int[,] Grid { get; set; }

        public GameEngine(int h, int w)
        {
            if(h <= 0 || w <= 0)
                throw new ArgumentOutOfRangeException("Indices cannot be negative!");

            Height = h;
            Width = w;
            Ticks = 0;
            Grid = new int[h, w];

            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                    Grid[j, i] = 0;
        }

        public int this[int y, int x]
        {
            get
            {
                if (IsOutOfBounds(y, x))
                    throw new ArgumentOutOfRangeException("Indices out of bound!");
                return Grid[y, x];
            }

            set
            {
                if(IsOutOfBounds(y, x))
                    throw new ArgumentOutOfRangeException("Indices out of bound!");
                Grid[y, x] = value;
            }
        }

        private bool IsOutOfBounds(int y, int x) =>
            (y < 0 || y >= Height || x < 0 || x >= Width);

        private int[,] CloneBoard()
        {
            int[,] boardCopy = new int[Height, Width];

            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                    boardCopy[j, i] = Grid[j, i];

            return boardCopy;
        }

        public void Tick()
        {
            int[,] boardCopy = CloneBoard();

            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                    boardCopy[j, i] = GetNextState(j, i);

            Grid = boardCopy;
            Ticks++;
        }

        private int GetNextState(int y, int x)
        {
            if (IsOutOfBounds(y, x))
                throw new ArgumentOutOfRangeException("Invalid coordinates!");

            int state = 0;

            for (int j = y - 1; j <= y + 1; j++)
                for (int i = x - 1; i <= x + 1; i++)
                    if (IsOutOfBounds(j, i) != true)
                        state = state + Grid[j, i];

            state = state - Grid[y, x];

            switch (state)
            {
                case 2:
                    return Grid[y, x];
                case 3:
                    return 1;
                default:
                    return 0;
            }
        }

        private bool CellLives(int y, int x)
        {
            bool returnValue = true;
            int livecount = 0;
            bool isLiving = Grid[y, x] == 1;
            for (int i = -1; i < 1; ++i)
            {
                for (int j = -1; j < 1; ++j)
                {
                    int offsetx = i + x;
                    int offsety = j + y;
                    if (!IsOutOfBounds(offsety, offsetx) && ((i != 0) && (j != 0)))
                    {
                        if (Grid[offsety, offsetx] == 1)
                            livecount++;
                    }
                }
            }
            if (isLiving)
            {
                if ((livecount < 2) || (livecount > 3))
                {
                    // dies
                    returnValue = false;
                }
            }


            return returnValue;
        }
    }
}
