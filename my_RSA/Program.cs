using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace my_RSA
{
    class Program
    {
        class ExtendedEuclideanResult
        {
            public int u1;
            public int u2;
            public int gcd;
        }
        class RSA
        {
            private BigInteger p = 61;
            private BigInteger q = 73;
            private BigInteger n;
            private BigInteger e;
            private BigInteger d;
            private BigInteger phi;
            private List<BigInteger> primaryNumbers = new List<BigInteger>();
            private List<BigInteger> possible_d = new List<BigInteger>();
            private BigInteger[] publickey = new BigInteger[2];
            private BigInteger[] privatekey = new BigInteger[2];
            public RSA() {
                generateKey();
            }
            private void generateKey()
            {
                n = p * q;
                phi = (p - 1) * (q - 1);
                for (int i = 0; i < phi; i++)
                {
                    if (IsPrime(i) && (n % i) != 0)
                        primaryNumbers.Add(i);
                }
                Random r = new Random();
                e = primaryNumbers[r.Next(0,primaryNumbers.Count - 1)];
                find_d();
                Console.WriteLine("e: " + e + "\nd: " + d + "\nn: " + n);
                publickey[0] = e;
                publickey[1] = n;
                privatekey[0] = (int)d;
                privatekey[1] = n;
            }
            //Extended Euclidean algorithm
            private void find_d()
            {

                long[] u;
                long[] v;
                long q, temp1, temp2, temp3;

                u = new long[] { 0, 0, 0 };
                v = new long[] { 0, 0, 0 };

                u[0] = 1; u[1] = 0; u[2] = (long)phi;
                v[0] = 0; v[1] = 1; v[2] = (long)e;

                while (v[2] != 0)
                {
                    q = (long)Math.Floor((decimal)u[2] / v[2]);
                    temp1 = u[0] - q * v[0];
                    temp2 = u[1] - q * v[1];
                    temp3 = u[2] - q * v[2];
                    u[0] = v[0];
                    u[1] = v[1];
                    u[2] = v[2];
                    v[0] = temp1;
                    v[1] = temp2;
                    v[2] = temp3;
                }
                if (u[1] < 0) d = (u[1] + phi);
                else d = (u[1]);

            }
            public uint encrypt(uint m)
            {
                BigInteger bint = BigInteger.ModPow(m, publickey[0],publickey[1]);
                return Convert.ToUInt32(bint.ToString());
            }
            public uint decrpyt(uint c)
            {
                BigInteger bint = BigInteger.ModPow(c, privatekey[0], privatekey[1]);
                return Convert.ToUInt32(bint.ToString());
            }
            public BigInteger[] getPublicKey()
            {
                return publickey;
            }
            public BigInteger[] getPrivateKey()
            {
                return privatekey;
            }
            private bool IsPrime(int candidate)
            {
                int i;
                for (i = 2; i <= candidate - 1; i++)
                {
                    if ((candidate % i) == 0)
                    {
                        return false;
                    }
                }
                if(i == candidate)
                {
                    return true;
                }
                return false;
            }

        }
        static void Main(string[] args)
        {
            RSA rsa = new RSA();
            int input = 98;
            uint ASCII = Convert.ToUInt32((uint)input);
            Console.WriteLine("Przed: " + ASCII);
            uint encrypted = rsa.encrypt(ASCII);
            Console.WriteLine("Zakodowane: " + encrypted);
            ASCII = rsa.decrpyt(encrypted);
            Console.WriteLine("Odkodowane: " + ASCII);
            Console.ReadKey();
        }
    }
}
