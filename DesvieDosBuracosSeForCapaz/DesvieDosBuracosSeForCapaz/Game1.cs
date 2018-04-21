using DesvieDosBuracosSeForCapaz.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DesvieDosBuracosSeForCapaz
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Objetos do jogo.
        Carro _carro;
        Buraco _buraco;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        protected override void Initialize()
        {
            // Inst�ncia os objetos do jogo definindo uma posi��o inicial.
            _carro = new Carro { Posicao = Vector2.Zero };
            _buraco = new Buraco { Posicao = new Vector2(0, 700) };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Carrega os sprites/imagens.
            // Utilizando o m�todo Load herdado da classe GameObject2D.
            _carro.Load(Content, "Sprite/carro");
            _buraco.Load(Content, "Sprite/buraco");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // Por enquanto o jogo n�o realiza nenhuma a��o.

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(); // Chamada obrigat�ria antes de desenhar os objetos

            // Desenha os objetos do jogo;
            // Utilizando o m�todo Draw herdado da classe GameObject2D.
            _carro.Draw(_spriteBatch);
            _buraco.Draw(_spriteBatch);

            _spriteBatch.End(); // Chamada obrigat�ria ap�s de desenhar os objetos

            base.Draw(gameTime);
        }
    }
}
