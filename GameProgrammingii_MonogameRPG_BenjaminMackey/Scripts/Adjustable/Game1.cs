using GameProgrammingii_MonogameRPG_BenjaminMackey.Content;
using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect effect;
        Texture2D texture;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            effect = new BasicEffect(GraphicsDevice);
            base.Initialize();

            //game object manager and stuff

            //Input Manager and stuff

            //Rendering controller and stuff
            RenderController.UpdateRenderVariables(_graphics);

            TechDemoTests test = new TechDemoTests();
            test.testMethod();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //texture = Content.Load<Texture2D>("blackSquare");
            // TODO: use this.Content to load your game content here
            SpriteBin.Add(Content.Load<Texture2D>("solidBlackSquare"), "solidBlackSquare");
            SpriteBin.Add(Content.Load<Texture2D>("blueSquare"), "blueSquare");

            //--

        }
        private bool temp = true;
        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //update player input
            InputManager.updateAll();
            //Run all update methods for gameobjects in the world
            ObjectManager.UpdateAllGameObjects();
            //Update renders
            RenderController.BuildNextRenderTable();

            //

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (RenderController._camera == null) return;
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            //effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.001f, RenderController._camera._renderDistance);

            SpriteEffects spriteEffects = new SpriteEffects();
            //ALL RENDERING=====================================================================
            if(RenderController._renderObjects != null)
            {
                foreach (RenderObjectData item in RenderController._renderObjects)
                {
                    //Debug.WriteLine("Rendering");
                    _spriteBatch.Draw(item._texture, 
                        item._position, 
                        item._cutOut, 
                        Color.White, 
                        (float)item._rotation,
                        new Microsoft.Xna.Framework.Vector2(item._cutOut.Width / 2f, item._cutOut.Height / 2f), //will change to use the renderFrom enum later
                        item._scale, 
                        spriteEffects, 
                        item._dist);
             
                }
            }
            //=====================================================================================
            _spriteBatch.End();
 
            base.Draw(gameTime);
        }
    }
}
