using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace DeepLab
{
    public class Player
    {
        public Vector2 Position; // Coordenadas X e Y

        private Vector2 _velocity; // Velocidad del jugador en X e Y
        private bool _isGrounded = false;
        private float _gravity = 800f;
        private float _groundY = 400f;      // Altura del suelo temporal

        private float _jumpTimer = 0f;       //Tiempo precionando el salto
        private float _maxJumpTime = 1.0f;    //timepo máximo que puede mantener el salto
        private bool _isChargingJump = false;

        private Texture2D _texture;

        private SpriteEffects _spriteEffect = SpriteEffects.None;

        private int _totalFrames = 2;
        private int _frameWidth;
        private int _frameHeight;
        private int _currentFrame = 0;
        private float _timer = 0f;
        private float _frameInterval = 0.15f;

        public Player(Vector2 startPosition)
        {
            Position = startPosition;
        }

        public void LoadContent(ContentManager content, string assetName)
        {
            _texture = content.Load<Texture2D>(assetName);
            _frameWidth = _texture.Width / _totalFrames;
            _frameHeight = _texture.Height;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            float speed = 200f;


            if (_isGrounded)
            {
                if (keyboardState.IsKeyUp(Keys.Space))
                {
                    if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                    {
                        Position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _spriteEffect = SpriteEffects.FlipHorizontally;
                    }
                    if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                    {
                        Position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _spriteEffect = SpriteEffects.None;
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    _isChargingJump = true;
                    _jumpTimer += deltaTime;

                    if (_jumpTimer > _maxJumpTime)
                        _jumpTimer = _maxJumpTime;
                }

                else if (_isChargingJump && keyboardState.IsKeyUp(Keys.Space))
                {
                    float jumpForce = -350f * (_jumpTimer / _maxJumpTime) - 200f;

                    _velocity.Y = jumpForce;
                    _isGrounded = false;
                    _isChargingJump = false;
                    _jumpTimer = 0f;
                }
            }

            if (!_isGrounded)
            {
                _velocity.Y += _gravity * deltaTime;
            }
            else
            {
                _velocity.Y = 0f;
            }

            Position.Y += _velocity.Y * deltaTime;

            if (Position.Y >= _groundY)
            {
                Position.Y = _groundY;
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _frameInterval)
            {
                _timer = 0f;
                _currentFrame = (_currentFrame + 1) % _totalFrames;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(_currentFrame * _frameWidth, 0, _frameWidth, _frameHeight);

            spriteBatch.Draw(_texture, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, _spriteEffect, 0f);
        }
    }
}