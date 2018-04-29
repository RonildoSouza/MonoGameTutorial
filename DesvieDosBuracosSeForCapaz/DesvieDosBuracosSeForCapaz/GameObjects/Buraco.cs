using Microsoft.Xna.Framework;
using System;

namespace DesvieDosBuracosSeForCapaz.GameObjects
{
    public class Buraco : GameObject2D
    {
        public int Dano { get; set; }

        /// <summary>
        /// Define uma posição aleatória inicial.
        /// </summary>
        /// <param name="graphics"></param>
        public void SetaPosicaoAleatoria(ref GraphicsDeviceManager graphics)
        {
            var random = new Random();

            // Gera um número aleatório entre 0 e o tamanho da tela menos a largura da textura.
            var posX = random.Next(0, graphics.GraphicsDevice.DisplayMode.Width - _textura.Width);

            // Gera um número aleatório entre 60% NEGATIVO da altura da tela e 0.
            var posY = random.Next((int)(graphics.GraphicsDevice.DisplayMode.Height * -0.6), 0);

            Posicao = new Vector2(posX, posY);
        }
    }
}