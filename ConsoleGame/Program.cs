using System.Threading;
using System.Linq;
using ConsoleGame;

public class Program
{   
    public static void Main()
    {
        var gameManager =_setUpGame();
        _mainLoop(gameManager);
    }

    private static GameManager _setUpGame()
    {
        var gameManager = new GameManager();
        gameManager.InitGame();
        return gameManager;
    }

    private static void _mainLoop(GameManager gameManager)
    {
        ulong counter = 0;
        while (gameManager.ProcessKeys())
        {
            gameManager.ProcessObjects(counter);
            Thread.Sleep(10);
            counter++;
        }
    }
}