namespace ConsoleGame
{
    public class DestoyableObject : BaseObject
    {
        public DestoyableObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            canBeDestroyed = true;
        }

        public override void Update()
        { }
    }
}
