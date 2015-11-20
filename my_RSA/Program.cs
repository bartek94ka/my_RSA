using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            private int p = 5;//19;
            private int q = 11;//13;
            private int n;
            private int e;
            private int d;
            private List<int> primaryNumbers = new List<int>();
            private List<int> possible_d = new List<int>();
            private int[] publickey = new int[2];
            private int[] privatekey = new int[2];
            public RSA() {
                generateKey();
            }
            private void generateKey()
            {
                n = p * q;
                int phi = (p - 1) * (q - 1);
                for (int i = 0; i < phi; i++)
                {
                    if (IsPrime(i) && (n % i) != 0)
                        primaryNumbers.Add(i);
                }
                Random r = new Random();
                e = primaryNumbers[r.Next(0,primaryNumbers.Count - 1)];
                //find_d(phi);
                //d = possible_d[r.Next(0, possible_d.Count - 1)];
                do
                {
                    d = ExtendedEuclidean(e % phi, phi).u1;
                } while (d > 0);
                Console.WriteLine("e: " + e + "\nd: " + d + "\nn: " + n);
                publickey[0] = e;
                publickey[1] = n;
                privatekey[0] = d;
                privatekey[1] = n;
            }
            private ExtendedEuclideanResult ExtendedEuclidean(int a, int b)
            {
                int u1 = 1;
                int u3 = a;
                int v1 = 0;
                int v3 = b;

                while (v3 > 0)
                {
                    int q0 = u3 / v3;
                    int q1 = u3 % v3;

                    int tmp = v1 * q0;
                    int tn = u1 - tmp;
                    u1 = v1;
                    v1 = tn;

                    u3 = v3;
                    v3 = q1;
                }

                int tmp2 = u1 * (a);
                tmp2 = u3 - (tmp2);
                int res = tmp2 / (b);

                ExtendedEuclideanResult result = new ExtendedEuclideanResult()
                {
                    u1 = u1,
                    u2 = res,
                    gcd = u3
                };

                return result;
            }
            private void find_d(int phi)
            {
                for (int i = 0; i < n; i++)
                {
                    d = e * i;
                    if (d % phi == 1)
                        possible_d.Add(d);
                }
            }
            public uint encrypt(uint m)
            {
                uint c = Convert.ToUInt32(Math.Pow(m, publickey[0]) % publickey[1]);
                return c;
            }
            public uint decrpyt(uint c)
            {
                UInt64 m = Convert.ToUInt64((Math.Pow(c, privatekey[0])) % privatekey[1]);
                return Convert.ToUInt32(m);
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
            //string input = "a";
            int input = 2;
            //char[] charinput = input.ToCharArray();
           // uint ASCII = Convert.ToUInt32(charinput[0]);
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
