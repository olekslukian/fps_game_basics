using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Simple3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private BasicEffect _basicEffect;
        private VertexPositionColor[] _vertices;
        private short[] _indices;

        private FpsCamera _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // TODO(olekslukian): Remove, when test level will be available
            _vertices =
            [
                new VertexPositionColor(new Vector3(-10, 0, -10), Color.Green),
                new VertexPositionColor(new Vector3(10, 0, -10), Color.Green),
                new VertexPositionColor(new Vector3(-10, 0, 10), Color.Green),
                new VertexPositionColor(new Vector3(10, 0, 10), Color.Green),
            ];

            _indices =
            [
                0, 1, 2,
                1, 3, 2
            ];

            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };

            _camera = new FpsCamera(new Vector3(0, 2, 10), GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _camera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _basicEffect.World = Matrix.Identity;
            _basicEffect.View = _camera.View;
            _basicEffect.Projection = _camera.Projection;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    _vertices, 0, 4,
                    _indices, 0, 2
                );
            }

            base.Draw(gameTime);
        }
    }
}
