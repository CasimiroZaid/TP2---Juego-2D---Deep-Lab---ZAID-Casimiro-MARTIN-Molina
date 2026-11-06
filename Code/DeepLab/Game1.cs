using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DeepLab;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player _player;

    // texturas
    private Texture2D _backgroundTexture;
    private Texture2D _logoTexture;
    private Texture2D _playButtonTexture;
    private Texture2D _settingsButtonTexture;
    private Texture2D _exitButtonTexture;

    private enum GameState
    {
        MainMenu,
        Playing
    }
    private GameState _currentState = GameState.MainMenu;

    private int _selectedMenuIndex = 0;
    private bool _isKeyReleased = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _player = new Player(new Vector2(100, 300));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _player.LoadContent(Content, "PlayerIdle");

        _backgroundTexture = Content.Load<Texture2D>("fondo1");
        _logoTexture = Content.Load<Texture2D>("DefLogo");
        _playButtonTexture = Content.Load<Texture2D>("playButton");
        _settingsButtonTexture = Content.Load<Texture2D>("settingsButton");
        _exitButtonTexture = Content.Load<Texture2D>("exitButton");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        switch (_currentState)
        {
            case GameState.MainMenu:
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    if (_isKeyReleased)
                    {
                        _selectedMenuIndex = (_selectedMenuIndex + 1) % 3;
                        _isKeyReleased = false;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    if (_isKeyReleased)
                    {
                        _selectedMenuIndex = (_selectedMenuIndex - 1 + 3) % 3;
                        _isKeyReleased = false;
                    }
                }

                else
                {
                    _isKeyReleased = true;
                }

                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    ExecuteMenuAction(_selectedMenuIndex);
                }

                Rectangle playButtonRect = new Rectangle(490, 320, 300, 60);
                Rectangle settingsButtonRect = new Rectangle(490, 400, 300, 60);
                Rectangle exitButtonRect = new Rectangle(490, 480, 300, 60);

                if (playButtonRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _currentState = GameState.Playing;
                }
                if (exitButtonRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    Exit();
                }
                break;

            case GameState.Playing:
                _player.Update(gameTime);
                break;
        }

        base.Update(gameTime);
    }

    private void ExecuteMenuAction(int index)
    {
        switch (index)
        {
            case 0: // Jugar
                _currentState = GameState.Playing;
                break;
            case 1: // configuracion
                break;
            case 2: // salir
                Exit();
                break;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        switch (_currentState)
        {
            case GameState.MainMenu:
                _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

                _spriteBatch.Draw(_logoTexture, new Vector2(440, 80), null, Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);

                Color playColor = (_selectedMenuIndex == 0) ? Color.Yellow : Color.White;
                Color settingsColor = (_selectedMenuIndex == 1) ? Color.Yellow : Color.White;
                Color exitColor = (_selectedMenuIndex == 2) ? Color.Yellow : Color.White;

                // x, y
                _spriteBatch.Draw(_playButtonTexture, new Vector2(490, 320), playColor);
                _spriteBatch.Draw(_settingsButtonTexture, new Vector2(490, 400), settingsColor);
                _spriteBatch.Draw(_exitButtonTexture, new Vector2(490, 480), exitColor);
                break;

            case GameState.Playing:
                _player.Draw(_spriteBatch);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}