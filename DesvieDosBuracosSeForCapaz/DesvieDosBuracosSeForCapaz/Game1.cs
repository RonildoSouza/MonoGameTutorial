using DesvieDosBuracosSeForCapaz.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
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
        SpriteFont _fonteGameOver;
        BotaoStart _btnStart;

        bool _start = false;

        int _descontoIPVA = 0;

        // Manipula os conteudos de sons
        Song _musica;
        SoundEffect _somColisao;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            Vector2 velocidade = new Vector2(0, 11);

            // Inst�ncia os objetos do jogo.
            _carro = new Carro { Resistencia = 100 };
            _buracos = new List<Buraco>();
            _cenario = new Cenario(_graphics, velocidade);
            _btnStart = new BotaoStart();

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

            // Carrega os sons
            _musica = Content.Load<Song>("Sons/Fringe");
            _somColisao = Content.Load<SoundEffect>("Sons/colisao");

            // Toca a m�sica repetidamente
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_musica);

            // Carrega o cen�rio
            _cenario.Load(Content, "Sprite/cenario");

            // Carrega o arquivo de fonte.
            _fonte = Content.Load<SpriteFont>("fonte");
            _fonteGameOver = Content.Load<SpriteFont>("fonteGameOver");

            // Carrega os sprites/imagens.
            // Utilizando o m�todo Load herdado da classe GameObject2D.
            _carro.Load(Content, "Sprite/carro");

            foreach (var buraco in _buracos)
                buraco.Load(Content, "Sprite/buraco");

            _btnStart.Load(Content, "Sprite/btn_start");

            // Seta a posi��o inicial dos objetos do jogo.
            int i = 0;
            do
            {
                // Posi��o buraco atual
                _buracos[i].SetaPosicaoAleatoria(ref _graphics);

                if (i > 0 && _buracos[i].Limites.Intersects(_buracos[i - 1].Limites))
                {
                    i--;
                    // Posi��o buraco anterior
                    _buracos[i].SetaPosicaoAleatoria(ref _graphics);
                }

                i++;
            } while (i < _buracos.Count);

            _carro.SetaPosicaoInicial(ref _graphics);

            _btnStart.SetaPosicao(ref _graphics);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            TouchCollection touchLocations = TouchPanel.GetState();

            if (_btnStart.Start(ref touchLocations) && _carro.Resistencia == 100)
                _start = true;

            if (_start)
            {
                #region Movimenta os objetos do jogo.

                _carro.Mover(ref touchLocations, ref _graphics);

                foreach (var buraco in _buracos)
                {
                    buraco.Posicao.Y += buraco.Velocidade.Y;

                    // A��es realizadas quando o buraco sai da tela.
                    if (buraco.Posicao.Y > _graphics.GraphicsDevice.DisplayMode.Height)
                    {
                        // N�o colidiu com o carro?
                        if (!buraco.JaColidiu)
                            _descontoIPVA += 50;

                        buraco.SetaPosicaoAleatoria(ref _graphics);
                        buraco.Dano = GetDanoAleatorio();
                        buraco.JaColidiu = false;
                    }

                    // Ocorreu uma colis�o?
                    if (buraco.ColidiuCom(ref _carro))
                    {
                        _somColisao.Play();
                        buraco.JaColidiu = true;
                        _carro.Resistencia -= buraco.Dano;
                    }

                    if (_carro.Resistencia <= 0)
                    {
                        _carro.Resistencia = 0;
                        _start = false;
                    }
                }

                #endregion

                // Atualiza a posi��o Y do cen�rio.
                _cenario.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(); // Chamada obrigat�ria antes de desenhar os objetos

            // Desenha o cen�rio
            _cenario.Draw(_spriteBatch);

            if (!_start && _carro.Resistencia == 100)
                _btnStart.Draw(_spriteBatch);

            if (_start)
            {
                // Desenha os objetos do jogo;
                // Utilizando o m�todo Draw herdado da classe GameObject2D.
                foreach (var buraco in _buracos)
                    buraco.Draw(_spriteBatch);

                _carro.Draw(_spriteBatch);
            }

            if (!_start && _carro.Resistencia <= 0)
            {
                var gameOver = "GAME\nOVER";
                var tamanhoString = _fonteGameOver.MeasureString(gameOver);

                _spriteBatch.DrawString(_fonteGameOver, gameOver,
                    new Vector2((_graphics.GraphicsDevice.DisplayMode.Width - tamanhoString.X) / 2,
                    (_graphics.GraphicsDevice.DisplayMode.Height - tamanhoString.Y) / 2),
                    Color.DarkRed);
            }

            #region Desenha textos de desconto IPVA e resist�ncia do carro

            _spriteBatch.DrawString(_fonte, $"RES. CARRO.: {_carro.Resistencia}", new Vector2(50, 10), Color.White);

            _spriteBatch.DrawString(_fonte, $"DESC. IPVA.: {_descontoIPVA:c}", new Vector2(50, 75), Color.White);

            #endregion

            _spriteBatch.End(); // Chamada obrigat�ria ap�s desenhar os objetos

            base.Draw(gameTime);
        }

        private int GetDanoAleatorio()
        {
            var random = new Random();
            return random.Next(1, 10);
        }
    }
}
