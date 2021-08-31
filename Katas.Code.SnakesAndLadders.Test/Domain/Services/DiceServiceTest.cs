using Katas.Code.SnakesAndLadders.Domain.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using Katas.Code.SnakesAndLadders.Domain.Entities;
using Moq;

namespace Katas.Code.SnakesAndLadders.Test
{
    public class DiceServiceTest
    {
        private readonly int[] _diceNumbers = { 1, 2, 3, 4, 5, 6 };
        private Mock<Dice> _dice;

        [SetUp]
        public void Setup()
        {
            _dice = new Mock<Dice>();
            _dice.Object.MinValue = 1;
            _dice.Object.MaxValue = 6;
        }

        [Test]
        public void Return_1_to_6_When_Call_The_Service()
        {
            var diceService = new DiceService(_dice.Object).GetRandomNumber();
            Assert.That(_diceNumbers, Has.Member(diceService));
            Assert.That(_diceNumbers, Does.Contain(diceService));  
        }
    }
}