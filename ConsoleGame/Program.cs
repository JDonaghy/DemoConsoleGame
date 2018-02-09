using System.Threading;
using ConsoleGame;
using System.Linq;

public class Program
{
    static int numRows = 29;
    static int numCols = 118;
    static GameManager gameManager = null;

    public static void Main()
    {
        _setup();
        _mainLoop();
    }

    private static void _setup()
    {
        gameManager = new GameManager(numRows, numCols);
        gameManager.InitGame();
    }

    private static void _mainLoop()
    {
        ulong counter = 0;
        while (gameManager.ProcessKeys())
        {
            gameManager.ProcessObjects(counter);
            if (counter % 10 == 0)
            {
                if (counter % 100 == 0)
                {
                    var destroyableObjectCount = gameManager.objList.Where(
                        obj => obj.CanBeDestroyed).ToList().Count;

                    if (!gameManager.GameOver && destroyableObjectCount < 5)
                    {
                        gameManager.AddRandomObject();
                    }
                }
            }
            Thread.Sleep(10);
            counter++;
        }
    }
}