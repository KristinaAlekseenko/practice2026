using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace task04
{
    public class Cruiser : ISpaceship
    {
        public int Speed => 50;
        public int FirePower => 100;

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
