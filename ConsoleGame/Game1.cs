using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System;

namespace ConsoleGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            _gameManager = new GameManager();
            _gameManager.NumColumns = 80;
            _gameManager.NumRows = 45;

            base.Initialize(); // Calls LoadContent
            
            _gameManager.InitGame();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures using Texture2D.FromStream (skipping MGCB pipeline for simplicity)
            // Ensure Content files are copied to output directory
            _gameManager.Textures["cursor"] = LoadTexture("cursor.png");
            _gameManager.Textures["cursor_fire"] = LoadTexture("cursor_fire.png");
            _gameManager.Textures["missile"] = LoadTexture("missile.png");
            _gameManager.Textures["sine"] = LoadTexture("sine.png");
            _gameManager.Textures["cosine"] = LoadTexture("cosine.png");
            _gameManager.Textures["simple"] = LoadTexture("simple.png");
            _gameManager.Textures["explosion"] = LoadTexture("explosion.png");
            _gameManager.Textures["explosion2"] = LoadTexture("explosion2.png");
            _gameManager.Textures["gameover"] = LoadTexture("gameover.png");
            _gameManager.Textures["background"] = LoadTexture("background.png");
            for (int i = 0; i < 10; i++)
            {
                _gameManager.Textures[$"num_{i}"] = LoadTexture($"num_{i}.png");
            }
        }
        
        private Texture2D LoadTexture(string name)
        {
            try 
            {
                using (var stream = File.OpenRead(Path.Combine(Content.RootDirectory, name)))
                {
                    return Texture2D.FromStream(GraphicsDevice, stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to load texture {name}: {e.Message}");
                // Return a 1x1 pink texture as fallback to prevent crash
                var t = new Texture2D(GraphicsDevice, 1, 1);
                t.SetData(new Color[] { Color.Magenta });
                return t;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!_gameManager.ProcessInput())
                Exit();
                
            _gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_gameManager.GameBackground);

            // Use NonPremultiplied for standard PNGs loaded via stream
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
            _gameManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
