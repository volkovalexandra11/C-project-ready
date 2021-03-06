﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Infrastructure
{
    public static class MathExtensions
    {
        public static double Cot(double x) => 1 / Math.Tan(x);

        public static double Acot(double x) => Math.Atan2(1, x);

        public static double Gcd(double a, double b)
        {
            var aInt = (int)Math.Round(a);
            var bInt = (int)Math.Round(b);
            return Gcd(aInt, bInt);
        }

        public static double Lcm(double a, double b)
        {
            var aInt = (int)Math.Round(a);
            var bInt = (int)Math.Round(b);
            return Lcm(aInt, bInt);
        }

        public static int Gcd(int a, int b)
        {
            while (true)
            {
                if (b == 0) return a;
                var temp = a;
                a = b;
                b = temp % b;
            }
        }

        public static int Lcm(int a, int b)
        {
            return a * b / Gcd(a, b);
        }

        public static double Log2(double x)
        {
            return Math.Log(x, 2);
        }
    }
}
