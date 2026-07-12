using System;
using System.Collections.Generic;
using System.Text;

namespace task04
{
    public class Fighter : ISpaceship
    {
        public int Speed => 100;
        public int FirePower => 70;

        public void MoveForward()
        {
            Console.WriteLine($"Вперед со скоростью {Speed}");
        }
        public void Rotate(int angle)
        {
            Console.WriteLine($"Поворот на {angle}");
        }
        public void Fire()
        {
            Console.WriteLine($"огонь с мощностью {FirePower}");
        }
    }

}
