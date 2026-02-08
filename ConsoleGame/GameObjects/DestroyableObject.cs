namespace ConsoleGame
{
    public class DestroyableObject : BaseObject
    {
        public DestroyableObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            canBeDestroyed = true;
        }

        public override void Update()
        { }
    }
}
