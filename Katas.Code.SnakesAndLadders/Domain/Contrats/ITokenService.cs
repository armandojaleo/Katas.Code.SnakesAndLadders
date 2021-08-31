using Katas.Code.SnakesAndLadders.Domain.Entities;
using System.Collections.Generic;

namespace Katas.Code.SnakesAndLadders.Domain.Contrats
{
    public interface ITokenService
    {
        Token CreateToken();
        List<Token> GetListTokens();
        Token UpdateTokenPosition(Token token, int newPosition);
    }
}
