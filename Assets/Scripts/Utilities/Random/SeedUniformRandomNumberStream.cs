
namespace Pacmania.Utilities.Random
{
    public class SeedUniformRandomNumberStream
    { 
        public int CurrentSeed { get; private set; }
        public int InitialSeed { get; private set; }
        private const int A = 48271;
        private const int M = 2147483647; 

        public SeedUniformRandomNumberStream(int seed)
        {
            InitialSeed = seed;
            CurrentSeed = seed;
        }

        public int NextSeed()
        {
            int q = M / A;
            int r = M % A;
            int low;
            int high;
            int temp;

            high = CurrentSeed / q;
            low = CurrentSeed % q;
            temp = A * low - r * high;

            if (temp > 0)
            {
                CurrentSeed = temp;
            }
            else
            {
                CurrentSeed = temp + M;
            }

            if (CurrentSeed < 0)
            {
                return -CurrentSeed;
            }

            return CurrentSeed;
        }

        public int Range(int min, int max)
        {
            int f = NextSeed();
            int d = (max - min)+1;
            f = (f % d) + min;
            return f;
        }
    }
}
