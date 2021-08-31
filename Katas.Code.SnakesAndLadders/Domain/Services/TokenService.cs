using System;
using Katas.Code.SnakesAndLadders.Domain.Contrats;
using Katas.Code.SnakesAndLadders.Domain.Entities;
using System.Collections.Generic;
using System.Drawing;

namespace Katas.Code.SnakesAndLadders.Domain.Services
{
    public class TokenService : ITokenService
    {
        public TokenService()
        {
        }

        public Token CreateToken()
        {
            var random = new Random();
            return new Token()
            {
                Guid = Guid.NewGuid(),
                Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)).ToString(),
                Position = 1,
                Winner = false
            };
        }

        public List<Token> GetListTokens()
        {
            return new List<Token>();
        }

        public Token UpdateTokenPosition(Token token, int newPosition)
        {
            token.Position = newPosition;
            return token;
        }
    }
}
