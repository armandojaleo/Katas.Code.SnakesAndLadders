using Katas.Code.SnakesAndLadders.Domain.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using Katas.Code.SnakesAndLadders.Domain.Entities;
using Moq;

namespace Katas.Code.SnakesAndLadders.Test
{
    class GameServiceTest
    {
        private Mock<Board> _board;

        [SetUp]
        public void Setup()
        {
            var snakes = new Dictionary<string, int>
            {
                {"16", 6},
                {"46", 25}
            };

            var ladders = new Dictionary<string, int>
            {
                {"2", 38},
                {"7", 14}
            };

            _board = new Mock<Board>();
            _board.Object.Size = 100;
            _board.Object.Snakes = snakes;
            _board.Object.Ladders = ladders;
        }

        [Test]
        public void Game_Start_Return_True()
        {
            var response = new GameService(_board.Object).Start();
            Assert.IsTrue(response);
        }

        [Test]
        public void Game_Stop_Return_False()
        {
            var response = new GameService(_board.Object).Stop();
            Assert.IsFalse(response);
        }

        [Test]
        [TestCase(1, 3, 4)]
        [TestCase(15, 1, 6)]
        [TestCase(45, 1, 25)]
        [TestCase(1, 1, 38)]
        [TestCase(1, 6, 14)]
        public void New_Box_Game_After_Snakes_And_Ladders_Return_Int(int position, int dice, int newPosition)
        {
            var response = new GameService(_board.Object).NewBoxGameAfterSnakesAndLadders(position, dice);
            Assert.AreEqual(newPosition, response);
        }
    }
}
