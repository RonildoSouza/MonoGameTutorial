using DesvieDosBuracosSeForCapaz.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace DesvieDosBuracosSeForCapaz
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Objetos do jogo.
        Carro _carro;
        List<Buraco> _buracos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // Instância os objetos do jogo.
            _carro = new Carro();
            _buracos = new List<Buraco>();

            // Cria os buracos com uma velocidade para o eixo X de Zero
            // e eixo Y de 11 adicionando o mesmo na lista.
            for (int i = 0; i < 5; i++)
                _buracos.Add(new Buraco { Velocidade = new Vector2(0, 11) });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Carrega os sprites/imagens.
            // Utilizando o método Load herdado da classe GameObject2D.
            _carro.Load(Content, "Sprite/carro");

            foreach (var buraco in _buracos)
                buraco.Load(Content, "Sprite/buraco");

            // Seta a posição inicial dos objetos do jogo.
            foreach (var buraco in _buracos)
                buraco.SetaPosicaoAleatoria(ref _graphics);

            _carro.SetaPosicaoInicial(ref _graphics);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            #region Movimenta os objetos do jogo.

            TouchCollection touchLocations = TouchPanel.GetState();

            _carro.Mover(ref touchLocations, ref _graphics);

            foreach (var buraco in _buracos)
            {
                buraco.Posicao.Y += buraco.Velocidade.Y;

                if (buraco.Posicao.Y > _graphics.GraphicsDevice.DisplayMode.Height)
                    buraco.SetaPosicaoAleatoria(ref _graphics);
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(); // Chamada obrigatória antes de desenhar os objetos

            // Desenha os objetos do jogo;
            // Utilizando o método Draw herdado da classe GameObject2D.
            foreach (var buraco in _buracos)
                buraco.Draw(_spriteBatch);

            _carro.Draw(_spriteBatch);

            _spriteBatch.End(); // Chamada obrigatória após de desenhar os objetos

            base.Draw(gameTime);
        }
    }
}
