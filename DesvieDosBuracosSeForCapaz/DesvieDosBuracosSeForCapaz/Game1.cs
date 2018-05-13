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
        Cenario _cenario;
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
            Vector2 velocidade = new Vector2(0, 11);

            // Instância os objetos do jogo.
            _carro = new Carro { Resistencia = 100 };
            _buracos = new List<Buraco>();
            _cenario = new Cenario(_graphics, velocidade);

            // Cria os buracos com uma velocidade para o eixo X de Zero
            // e eixo Y de 11 adicionando o mesmo na lista.
            for (int i = 0; i < 5; i++)
            {
                _buracos.Add(new Buraco
                {
                    Velocidade = velocidade,
                    Dano = GetDanoAleatorio()
                });
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Carrega o cenário
            _cenario.Load(Content, "Sprite/cenario");

            // Carrega o arquivo de fonte.
            _fonte = Content.Load<SpriteFont>("fonte");

            // Carrega os sprites/imagens.
            // Utilizando o método Load herdado da classe GameObject2D.
            _carro.Load(Content, "Sprite/carro");

            foreach (var buraco in _buracos)
                buraco.Load(Content, "Sprite/buraco");

            // Seta a posição inicial dos objetos do jogo.
            int i = 0;
            do
            {
                // Posição buraco atual
                _buracos[i].SetaPosicaoAleatoria(ref _graphics);

                if (i > 0 && _buracos[i].Limites.Intersects(_buracos[i - 1].Limites))
                {
                    i--;
                    // Posição buraco anterior
                    _buracos[i].SetaPosicaoAleatoria(ref _graphics);
                }

                i++;
            } while (i < _buracos.Count);

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

                // Ações realizadas quando o buraco sai da tela.
                if (buraco.Posicao.Y > _graphics.GraphicsDevice.DisplayMode.Height)
                {
                    // Não colidiu com o carro?
                    if (!buraco.JaColidiu)
                        _descontoIPVA += 50;

                    buraco.SetaPosicaoAleatoria(ref _graphics);
                    buraco.Dano = GetDanoAleatorio();
                    buraco.JaColidiu = false;
                }

                // Ocorreu uma colisão?
                if (buraco.ColidiuCom(ref _carro))
                {
                    buraco.JaColidiu = true;
                    _carro.Resistencia -= buraco.Dano;
                }
            }

            #endregion

            // Atualiza a posição Y do cenário.
            _cenario.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(); // Chamada obrigatória antes de desenhar os objetos

            // Desenha o cenário
            _cenario.Draw(_spriteBatch);

            // Desenha os objetos do jogo;
            // Utilizando o método Draw herdado da classe GameObject2D.
            foreach (var buraco in _buracos)
                buraco.Draw(_spriteBatch);

            _carro.Draw(_spriteBatch);

            #region Desenha textos de desconto IPVA e resistência do carro

            _spriteBatch.DrawString(_fonte, $"RES. CARRO.: {_carro.Resistencia}",
                new Vector2(50, 10), Color.White);

            _spriteBatch.DrawString(_fonte, $"DESC. IPVA.: {_descontoIPVA:c}",
                new Vector2(50, 75), Color.White);

            #endregion 

            _spriteBatch.End(); // Chamada obrigatória após desenhar os objetos

            base.Draw(gameTime);
        }

        private int GetDanoAleatorio()
        {
            var random = new Random();
            return random.Next(1, 10);
        }
    }
}
