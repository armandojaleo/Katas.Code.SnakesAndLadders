using Katas.Code.SnakesAndLadders.Domain.Entities;

namespace Katas.Code.SnakesAndLadders.Domain.Contrats
{
    public interface IGameService
    {
        bool Start();
        bool Stop();
        int NewBoxGameAfterSnakesAndLadders(int position, int newPosition);
        int GetNumberOfBoxesGames();
    }
}
