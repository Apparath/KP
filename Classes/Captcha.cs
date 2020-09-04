using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KP.Classes
{
    class Captcha
    {
        /// <summary>
        /// Метод обновления капчи и очистки поля ввода проверки
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="textBox"></param>
        public static void Refresh(TextBlock textBlock, TextBox textBox)
        {
            textBlock.Text = "";
            textBox.Text = "";

            string symb = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

            char[] vs = symb.ToCharArray();
            Brush[] backs = { Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.LightBlue, Brushes.Blue, Brushes.Purple };
            Brush[] fores = { Brushes.Black, Brushes.Gray, Brushes.Brown, Brushes.Gold, Brushes.White };

            Random rnd = new Random();

            for (int i = 0; i < rnd.Next(4, 7); i++)
            {
                textBlock.Text += vs[rnd.Next(vs.Length)];
                textBlock.Background = backs[rnd.Next(backs.Length)];
                textBlock.Foreground = fores[rnd.Next(fores.Length)];
            }
        }

        /// <summary>
        /// Метод появления капчи после трех неправильных попыток
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="textBox"></param>
        /// <param name="button"></param>
        /// <param name="check"></param>
        public static void Check(TextBlock textBlock, TextBox textBox, Button button, bool check)
        {
            if (check == true)
            {
                textBlock.Visibility = Visibility.Hidden;
                button.Visibility = Visibility.Hidden;
                textBox.Visibility = Visibility.Hidden;
            }
            else if (check == false)
            {
                textBlock.Visibility = Visibility.Visible;
                button.Visibility = Visibility.Visible;
                textBox.Visibility = Visibility.Visible;

                Refresh(textBlock, textBox);
            }
        }
    }
}
