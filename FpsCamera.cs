using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Simple3D
{
    public class FpsCamera
    {
        private Vector3 _position;
        private float _yaw;
        private float _pitch;

        private float _moveSpeed = 10f;
        private float _lookSpeed = 0.01f;

        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;

        private MouseState _previousMouseState;

        public Matrix View => _viewMatrix;
        public Matrix Projection => _projectionMatrix;
        public Vector3 Position => _position;

        public FpsCamera(Vector3 initialPosition, GraphicsDevice graphicsDevice)
        {
            _position = initialPosition;
            _yaw = 0;
            _pitch = 0;

            float aspectRatio = graphicsDevice.Viewport.AspectRatio;

            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, aspectRatio, 0.1f, 1000f);

            UpdateViewMatrix();

            _previousMouseState = Mouse.GetState();
        }

        private void UpdateViewMatrix()
        {
            Vector3 forward = Vector3.Transform(Vector3.Forward,
                Matrix.CreateRotationX(_pitch) * Matrix.CreateRotationY(_yaw));
            forward.Normalize();

            Vector3 up = Vector3.Up;
            _viewMatrix = Matrix.CreateLookAt(_position, _position + forward, up);
        }

        public void Update(GameTime gameTime)
        {

            // TODO(olekslukian): Move controls to separate class

            KeyboardState keyboardState = Keyboard.GetState();
            Vector3 moveDirection = Vector3.Zero;

            if (keyboardState.IsKeyDown(Keys.W))
                moveDirection += Vector3.Forward;
            if (keyboardState.IsKeyDown(Keys.S))
                moveDirection += Vector3.Backward;
            if (keyboardState.IsKeyDown(Keys.A))
                moveDirection += Vector3.Left;
            if (keyboardState.IsKeyDown(Keys.D))
                moveDirection += Vector3.Right;

            if (moveDirection != Vector3.Zero)
            {
                moveDirection = Vector3.Transform(moveDirection,
                    Matrix.CreateRotationY(_yaw));
                moveDirection.Normalize();

                _position += moveDirection * _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != _previousMouseState)
            {
                int deltaX = currentMouseState.X - _previousMouseState.X;
                int deltaY = currentMouseState.Y - _previousMouseState.Y;

                _yaw -= deltaX * _lookSpeed;
                _pitch -= deltaY * _lookSpeed;

                _pitch = MathHelper.Clamp(_pitch, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

                _previousMouseState = currentMouseState;
            }

            UpdateViewMatrix();
        }
    }
}
