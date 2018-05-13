using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DesvieDosBuracosSeForCapaz.GameObjects
{
    public class Cenario : GameObject2D
    {
        int _largura;
        int _altura;

        public Cenario(GraphicsDeviceManager graphics, Vector2 velocidade)
        {
            Velocidade = velocidade;

            _largura = graphics.GraphicsDevice.DisplayMode.Width;
            _altura = graphics.GraphicsDevice.DisplayMode.Height;
        }

        /// <summary>
        /// Atualiza a posição Y de acordo com a velocidade Y
        /// </summary>
        public void Update()
        {
            Posicao.Y += Velocidade.Y;

            if (Posicao.Y >= _altura)
                Posicao.Y = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Desenha o cenario dentro da tela do dispositivo
            // Ex.: Eixo X = 0, Eixo Y = 0
            spriteBatch.Draw(_textura, new Rectangle(0, (int)Posicao.Y, _largura, _altura), Color.White);

            // Desenha o cenario fora da tela do dispositivo
            // Ex.: Eixo X = 0, Eixo Y = (0 - alturaDaTelaDoDispositivo)
            spriteBatch.Draw(_textura, new Rectangle(0, (int)(Posicao.Y - _altura), _largura, _altura), Color.White);
        }
    }
}