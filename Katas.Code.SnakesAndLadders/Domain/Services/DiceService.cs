using Katas.Code.SnakesAndLadders.Domain.Contrats;
using Microsoft.Extensions.Configuration;
using System;
using Katas.Code.SnakesAndLadders.Domain.Entities;

namespace Katas.Code.SnakesAndLadders.Domain.Services
{
    public class DiceService: IDiceService
    {
        private readonly int _diceMinValue;
        private readonly int _diceMaxValue;
        
        public DiceService(Dice dice)
        {
            _diceMinValue = dice.MinValue;
            _diceMaxValue = dice.MaxValue;
        }

        public int GetRandomNumber() => new Random().Next(_diceMinValue, _diceMaxValue);
    }
}
