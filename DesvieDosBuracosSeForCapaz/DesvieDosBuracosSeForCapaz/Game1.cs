using DesvieDosBuracosSeForCapaz.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
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
        SpriteFont _fonte;

        int _descontoIPVA = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // Instância os objetos do jogo.
            _carro = new Carro { Resistencia = 100 };
            _buracos = new List<Buraco>();

            // Cria os buracos com uma velocidade para o eixo X de Zero
            // e eixo Y de 11 adicionando o mesmo na lista.
            for (int i = 0; i < 5; i++)
            {
                _buracos.Add(new Buraco
                {
                    Velocidade = new Vector2(0, 11),
                    Dano = GetDanoAleatorio()
                });
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _fonte = Content.Load<SpriteFont>("fonte");

            // Carrega os sprites/imagens.
            // Utilizando o método Load herdado da classe GameObject2D.
            _carro.Load(Content, "Sprite/carro");

            foreach (var buraco in _buracos)
                buraco.Load(Content, "Sprite/buraco");

            // Seta a posição inicial dos objetos do jogo.
            //foreach (var buraco in _buracos)
            //    buraco.SetaPosicaoAleatoria(ref _graphics);

            for (int i = 0; i < _buracos.Count; i++)
            {
                do
                {
                    _buracos[i].SetaPosicaoAleatoria(ref _graphics);
                } while (i > 0 && _buracos[i].Limites.Intersects(_buracos[i - 1].Limites));
            }

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
                {
                    buraco.SetaPosicaoAleatoria(ref _graphics);
                    buraco.JaColidiu = false;
                    buraco.Dano = GetDanoAleatorio();
                }

                if (buraco.ColidiuCom(ref _carro))
                {
                    buraco.JaColidiu = true;
                    _carro.Resistencia -= buraco.Dano;
                }
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            string debug = "";
            debug += $"Carro {_carro.Limites}\n";
            debug += $"Carro Resitencia {_carro.Resistencia}\n\n";
            debug += $"Buraco 0 {_buracos[0].Limites}\n";
            debug += $"Buraco 0 Dano {_buracos[0].Dano}\n";
            debug += $"Buraco 1 {_buracos[1].Limites}\n";
            debug += $"Buraco 2 {_buracos[2].Limites}\n";
            debug += $"Colidiu 0 {_buracos[0].ColidiuCom(ref _carro)}\n";
            debug += $"Colidiu 1 {_buracos[1].ColidiuCom(ref _carro)}\n";
            debug += $"Colidiu 2 {_buracos[2].ColidiuCom(ref _carro)}\n";

            _spriteBatch.Begin(); // Chamada obrigatória antes de desenhar os objetos

            _spriteBatch.DrawString(_fonte, debug, Vector2.One, Color.Black);

            _spriteBatch.DrawString(_fonte, _descontoIPVA.ToString(),
                new Vector2(0, 500),
                Color.Black);

            // Desenha os objetos do jogo;
            // Utilizando o método Draw herdado da classe GameObject2D.
            for (int i = 0; i < _buracos.Count; i++)
            {
                var buraco = _buracos[i];

                buraco.Draw(_spriteBatch);
                _spriteBatch.DrawString(_fonte, i.ToString(), buraco.Posicao, Color.White);
            }

            _carro.Draw(_spriteBatch);

            _spriteBatch.End(); // Chamada obrigatória após de desenhar os objetos

            base.Draw(gameTime);
        }

        private int GetDanoAleatorio()
        {
            var random = new Random();
            return random.Next(1, 10);
        }
    }
}
