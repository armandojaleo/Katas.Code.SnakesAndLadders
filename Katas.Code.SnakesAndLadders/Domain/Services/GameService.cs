using Katas.Code.SnakesAndLadders.Domain.Contrats;
using Katas.Code.SnakesAndLadders.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Katas.Code.SnakesAndLadders.Domain.Services
{
    public class GameService : IGameService
    {
        private readonly int _size;
        private readonly Dictionary<string, int> _snakes;
        private readonly Dictionary<string, int> _ladders;
        private Board _boardBoxes;

        public JObject Board = null;

        public GameService(Board board)
        {
            _size = board.Size;
            _snakes = board.Snakes;
            _ladders = board.Ladders;
        }

        private Board NewBoard()
        {
            _boardBoxes = new Board()
            {
                Guid = Guid.NewGuid(),
                Size = _size,
                Snakes = _snakes,
                Ladders = _ladders
            };
            return _boardBoxes;
        }

        private JObject BoardGeneration()
        {
            JObject board = JObject.FromObject(NewBoard());
            Board = board;
            return Board;
        }

        private bool DeleteBoard()
        {
            _boardBoxes = null;
            Board = null;
            return false;
        }

        public bool Start()
        {
            var response = BoardGeneration();
            if (response.GetType() == typeof(JObject))
                return true;
            return false;
        }

        public bool Stop()
        {
            var response = DeleteBoard();
            if (!response)
                return false;
            return true;
        }

        public int NewBoxGameAfterSnakesAndLadders(int position, int diceNumber)
        {
            var newPosition = position + diceNumber;
            var newPositionSnakes = _snakes.FirstOrDefault(f => f.Key == newPosition.ToString()).Value;
            var newPositionLadders = _ladders.FirstOrDefault(f => f.Key == newPosition.ToString()).Value;
            var size = _size;
            if (newPosition > size)
                return position;
            if (newPositionSnakes == 0 && newPositionLadders == 0)
                return newPosition;
            if (newPositionSnakes != position && newPositionLadders == 0)
                return newPositionSnakes;
            if (newPositionSnakes == 0 && newPositionLadders != position)
                return newPositionLadders;
            return position;
        }

        public int GetNumberOfBoxesGames()
        {
            return _boardBoxes.Size;
        }
    }
}
