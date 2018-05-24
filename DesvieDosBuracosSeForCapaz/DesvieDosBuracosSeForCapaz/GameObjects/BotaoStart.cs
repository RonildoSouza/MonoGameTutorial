using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace DesvieDosBuracosSeForCapaz.GameObjects
{
    public class BotaoStart : GameObject2D
    {
        public bool Start(ref TouchCollection touchLocations)
        {
            bool touchEmCimaDoBotao = false;

            foreach (TouchLocation touchLocation in touchLocations)
            {
                if (touchLocation.State == TouchLocationState.Released)
                {
                    // Condicional para verificar se o touch do usuário
                    // na tela do dispositivo está ocorrendo em cima do botão start
                    touchEmCimaDoBotao = Limites.Intersects(new Rectangle(
                        (int)touchLocation.Position.X,
                        (int)touchLocation.Position.Y, 0, 0));
                }
            }

            return touchEmCimaDoBotao;
        }

        /// <summary>
        /// Posiciona o botão no centro da tela
        /// </summary>
        /// <param name="graphics"></param>
        public void SetaPosicao(ref GraphicsDeviceManager graphics)
        {
            var posX = (graphics.GraphicsDevice.DisplayMode.Width / 2 - _textura.Width / 2);
            var posY = (graphics.GraphicsDevice.DisplayMode.Height / 2 - _textura.Height / 2);
            Posicao = new Vector2(posX, posY);
        }
    }
}