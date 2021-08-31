using System;
using System.Collections.Generic;

namespace Katas.Code.SnakesAndLadders.Domain.Entities
{
    public class Board
    {
        public Guid Guid;
        public int Size;
        public Dictionary<string, int> Snakes;
        public Dictionary<string, int> Ladders;
    }
}
