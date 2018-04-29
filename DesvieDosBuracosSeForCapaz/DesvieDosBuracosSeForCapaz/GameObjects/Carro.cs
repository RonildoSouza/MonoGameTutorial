using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace DesvieDosBuracosSeForCapaz.GameObjects
{
    public class Carro : GameObject2D
    {
        public int Resistencia { get; set; }

        /// <summary>
        /// Move o carro quando o usuário mantem o dedo pressionado na tela e arrasta para a direção desejada.
        /// </summary>
        /// <param name="touchCollection"></param>
        /// <param name="graphics"></param>
        public void Mover(ref TouchCollection touchCollection, ref GraphicsDeviceManager graphics)
        {
            // Variavel criada para armazenar a localização anterior 
            // da ação de touch do usuário na tela do dispositivo.
            TouchLocation touchLocationAnterior = new TouchLocation();

            // O objeto touchCollection traz a localização atual 
            // da ação de touch do usuário na tela do dispositivo.
            foreach (TouchLocation touchLocationAtual in touchCollection)
            {
                touchLocationAtual.TryGetPreviousLocation(out touchLocationAnterior);

                if (touchLocationAtual.State != TouchLocationState.Released &&
                    touchLocationAnterior.State == TouchLocationState.Moved)
                {
                    // Condicional para verificar se o touch do usuário
                    // na tela do dispositivo está ocorrendo em cima do carro
                    bool touchEmCimaDoCarro = Limites.Intersects(new Rectangle(
                        (int)touchLocationAtual.Position.X,
                        (int)touchLocationAtual.Position.Y, 0, 0));

                    if (touchEmCimaDoCarro)
                    {
                        Posicao.X = MathHelper.Clamp(
                            (touchLocationAtual.Position.X - _textura.Width / 2),       // Posição X do touch do usuário menos a metade da largura da textura;
                            0,                                                          // Valor minimo para X.
                            graphics.GraphicsDevice.DisplayMode.Width - _textura.Width  // Valor maximo para X. Largura da tela do dispositivo menos a largura da textura.
                            );

                        Posicao.Y = MathHelper.Clamp(
                            (touchLocationAtual.Position.Y - _textura.Height / 2),          // Posição Y do touch do usuário menos a metade da altura da textura;
                           (float)(graphics.GraphicsDevice.DisplayMode.Height * 0.2),       // Valor minimo para Y. 20% da altura da tela do dispositivo.
                            (graphics.GraphicsDevice.DisplayMode.Height - _textura.Height)  // Valor maximo para Y. Altura da tela do dispositivo menos a altura da textura.
                            );
                    }
                }
            }
        }

        /// <summary>
        /// Posiciona o carro na parte inferior centralizado ao centro da tela.
        /// </summary>
        /// <param name="graphics"></param>
        public void SetaPosicaoInicial(ref GraphicsDeviceManager graphics)
        {
            var posX = (graphics.GraphicsDevice.DisplayMode.Width / 2 - _textura.Width / 2);
            var posY = (graphics.GraphicsDevice.DisplayMode.Height - _textura.Height);
            Posicao = new Vector2(posX, posY);
        }
    }
}