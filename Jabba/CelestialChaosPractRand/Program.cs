using EnderPi.Cryptography;
using EnderPi.Random;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace CelestialChaosPractRand
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0 && string.Compare(args[0], "sha32", StringComparison.OrdinalIgnoreCase) == 0)
            {
                int differentialOffset = 0;
                bool isDifferential = false;
                var diffArg = args.FirstOrDefault(x => x.StartsWith("-d"));
                if (diffArg != null)
                {
                    isDifferential = true;
                    differentialOffset = int.Parse(diffArg.Substring(2));
                }                
                File.AppendAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\ChaosRunRecord.txt", $"Running Chaos Oracle In bits 32 Mode {DateTime.Now} with differential ={isDifferential}, and differentialOffset={differentialOffset}" + Environment.NewLine);
                var generator = new Sha256Tester32bit(isDifferential, differentialOffset);
                using (Stream myOutStream = Console.OpenStandardOutput())
                {
                    ulong[] randomData = new ulong[1024];
                    byte[] randomBytes = new byte[1024 * 8];
                    while (true)
                    {
                        for (int i = 0; i < randomData.Length; i++)
                        {
                            randomData[i] = generator.Next();
                        }
                        Buffer.BlockCopy(randomData, 0, randomBytes, 0, randomBytes.Length);
                        myOutStream.Write(randomBytes, 0, randomData.Length * 8);
                    }
                }
            } 
            else if (args.Length != 0 && string.Compare(args[0], "spectrum", StringComparison.OrdinalIgnoreCase) == 0)
            {
                File.AppendAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\ChaosRunRecord.txt", $"Running Chaos Oracle In Spectrum Mode {DateTime.Now}" + Environment.NewLine);
                EnderLcg lcg = new EnderLcg();
                EnderPi.Random.RandomNumberGenerator rng = new EnderPi.Random.RandomNumberGenerator(lcg);
                rng.SeedRandom();
                var initial = rng.Nextulong();
                var masks = new List<ulong>();
                var singleMasks = new List<ulong>();
                for (int i=0; i < 64; i++)
                {
                    singleMasks.Add(1ul << i);
                    masks.Add(1ul << i);
                }
                for (int i=0; i <64; i++)
                {
                    for (int j=0; j < 64; j++)
                    {
                        if (i!=j)
                        {
                            masks.Add(singleMasks[i] | singleMasks[j]);
                        }
                    }
                }
                for (int i = 0; i < 64; i++)
                {
                    for (int j = 0; j < 64; j++)
                    {
                        for (int k = 0; k < 64; k++)
                        {
                            if (i != j && j != k && i != k)
                            {
                                masks.Add(singleMasks[i] | singleMasks[j] | singleMasks[k]);
                            }
                        }
                    }
                }
                var sb = new StringBuilder();
                var initialHash = ChaosOracle.BlendIntegers(1, initial);
                ulong[] hamming = new ulong[65];
                foreach(var mask in masks)
                {
                    var hash = ChaosOracle.BlendIntegers(1, initial ^ mask);
                    var bitsDifferent = BitOperations.PopCount(hash ^ initialHash);
                    hamming[bitsDifferent]++;
                }
                sb.AppendLine($"bitsdifferent,count");
                for (int i=0; i < hamming.Length; i++)
                {
                    sb.AppendLine($"{i},{hamming[i]}");
                }
                File.WriteAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\HamSpectrum.txt", sb.ToString());
            }
            else if (args.Length != 0 && string.Compare(args[0], "bits40", StringComparison.OrdinalIgnoreCase) == 0)
            {
                int rounds = 10;
                int differentialOffset = 0;
                bool isDifferential = false;
                var diffArg = args.FirstOrDefault(x => x.StartsWith("-d"));
                if (diffArg != null)
                {
                    isDifferential = true;
                    differentialOffset = int.Parse(diffArg.Substring(2));
                }
                var roundsArg = args.FirstOrDefault(x => x.StartsWith("-r"));
                if (roundsArg != null)
                {
                    rounds = int.Parse(roundsArg.Substring(2));
                }
                File.AppendAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\ChaosRunRecord.txt", $"Running Chaos Oracle In bits 40 Mode {DateTime.Now} with differential ={isDifferential}, rounds = {rounds}, and differentialOffset={differentialOffset}" + Environment.NewLine);
                var generator = new ChaosOracle40(rounds, isDifferential, differentialOffset);
                using (Stream myOutStream = Console.OpenStandardOutput())
                {
                    ulong[] randomData = new ulong[1024];
                    byte[] randomBytes = new byte[1024 * 8];
                    while (true)
                    {
                        for (int i = 0; i < randomData.Length; i++)
                        {
                            randomData[i] = generator.Next();
                        }
                        Buffer.BlockCopy(randomData, 0, randomBytes, 0, randomBytes.Length);
                        myOutStream.Write(randomBytes, 0, randomData.Length * 8);
                    }
                }
            }
            else if (args.Length != 0 && string.Compare(args[0], "bits32", StringComparison.OrdinalIgnoreCase) == 0)
            {
                int rounds = 10;
                int differentialOffset = 0;
                bool isDifferential = false;
                var diffArg = args.FirstOrDefault(x => x.StartsWith("-d"));
                if (diffArg != null)
                {
                    isDifferential = true;
                    differentialOffset = int.Parse(diffArg.Substring(2));
                }
                var roundsArg = args.FirstOrDefault(x => x.StartsWith("-r"));
                if (roundsArg != null)
                {                    
                    rounds = int.Parse(roundsArg.Substring(2));
                }
                File.AppendAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\ChaosRunRecord.txt", $"Running Chaos Oracle In bits 32 Mode {DateTime.Now} with differential ={isDifferential}, rounds = {rounds}, and differentialOffset={differentialOffset}" + Environment.NewLine);
                var generator = new ChaosOracle32(rounds, isDifferential, differentialOffset);
                using (Stream myOutStream = Console.OpenStandardOutput())
                {
                    ulong[] randomData = new ulong[1024];
                    byte[] randomBytes = new byte[1024 * 8];
                    while (true)
                    {
                        for (int i = 0; i < randomData.Length; i++)
                        {
                            randomData[i] = generator.Next();
                        }
                        Buffer.BlockCopy(randomData, 0, randomBytes, 0, randomBytes.Length);
                        myOutStream.Write(randomBytes, 0, randomData.Length * 8);
                    }
                }
            }
            else if (args.Length != 0 && string.Compare(args[0], "bits64", StringComparison.OrdinalIgnoreCase) == 0)
            {
                int rounds = 10;
                int differentialOffset = 0;
                bool isDifferential = false;
                var diffArg = args.FirstOrDefault(x => x.StartsWith("-d"));
                if (diffArg != null)
                {
                    isDifferential = true;
                    differentialOffset = int.Parse(diffArg.Substring(2));
                }
                var roundsArg = args.FirstOrDefault(x => x.StartsWith("-r"));
                if (roundsArg != null)
                {
                    rounds = int.Parse(roundsArg.Substring(2));
                }
                File.AppendAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\ChaosRunRecord.txt", $"Running Chaos Oracle In Normal Mode {DateTime.Now} with differential {isDifferential} offset {differentialOffset} and rounds {rounds}" + Environment.NewLine);
                var generator = new ChaosOracle64(rounds, isDifferential, differentialOffset);
                using (Stream myOutStream = Console.OpenStandardOutput())
                {
                    ulong[] randomData = new ulong[1024];
                    byte[] randomBytes = new byte[1024 * 8];
                    while (true)
                    {
                        for (int i = 0; i < randomData.Length; i++)
                        {                            
                            randomData[i] = generator.Next();                            
                        }
                        Buffer.BlockCopy(randomData, 0, randomBytes, 0, randomBytes.Length);
                        myOutStream.Write(randomBytes, 0, randomData.Length * 8);
                    }
                }
            }
            else if (args.Length != 0 && string.Compare(args[0], "enderlcg", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var generator = new EnderLcg();
                generator.Seed(BitConverter.ToUInt64(System.Security.Cryptography.RandomNumberGenerator.GetBytes(8)));
                File.AppendAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\ChaosRunRecord.txt", $"Running EnderLcg 64 bit {DateTime.Now}" + Environment.NewLine);                
                using (Stream myOutStream = Console.OpenStandardOutput())
                {
                    ulong[] randomData = new ulong[1024];
                    byte[] randomBytes = new byte[1024 * 8];
                    while (true)
                    {
                        for (int i = 0; i < randomData.Length; i++)
                        {
                            randomData[i] = generator.Nextulong();
                        }
                        Buffer.BlockCopy(randomData, 0, randomBytes, 0, randomBytes.Length);
                        myOutStream.Write(randomBytes, 0, randomData.Length * 8);
                    }
                }
            }
            else if (args.Length != 0 && string.Compare(args[0], "speedtest", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ulong seed = BitConverter.ToUInt64(System.Security.Cryptography.RandomNumberGenerator.GetBytes(8));
                ulong sum = 0;
                var sw = Stopwatch.StartNew();
                ulong max = 1000000000;
                for (ulong i=0; i < max; i++)
                {
                    sum += ChaosOracle.BlendIntegers(seed, i);
                }
                sw.Stop();
                ulong numsPerSecond =1000 * max / Convert.ToUInt64(sw.ElapsedMilliseconds);
                Console.WriteLine($"Generated {max} numbers in {sw.ElapsedMilliseconds} milliseconds, {numsPerSecond} numbers per second or {numsPerSecond*8} bytes per second");
                Console.ReadKey();
            }
            else if (args.Length != 0 && string.Compare(args[0], "speedtestmulti", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ulong seed = BitConverter.ToUInt64(System.Security.Cryptography.RandomNumberGenerator.GetBytes(8));
                ulong sum = 0;
                var sw = Stopwatch.StartNew();
                ulong max = 1000000000;
                for (ulong i = 0; i < max; i++)
                {
                    sum += ChaosOracleFast.BlendIntegers(seed, i);
                }
                sw.Stop();
                ulong numsPerSecond = 1000 * max / Convert.ToUInt64(sw.ElapsedMilliseconds);
                Console.WriteLine($"Generated {max} numbers in {sw.ElapsedMilliseconds} milliseconds, {numsPerSecond} numbers per second or {numsPerSecond * 8} bytes per second");
                Console.ReadKey();
            }
            else if (args.Length != 0 && string.Compare(args[0], "speedtestpcg", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ulong seed = BitConverter.ToUInt64(System.Security.Cryptography.RandomNumberGenerator.GetBytes(8));
                var pcg = new Pcg32();
                pcg.Seed(seed);
                ulong sum = 0;
                var sw = Stopwatch.StartNew();
                ulong max = 1000000000;
                for (ulong i = 0; i < max; i++)
                {
                    sum += pcg.Nextulong();
                }
                sw.Stop();
                ulong numsPerSecond = 1000 * max / Convert.ToUInt64(sw.ElapsedMilliseconds);
                Console.WriteLine($"Generated {max} numbers in {sw.ElapsedMilliseconds} milliseconds, {numsPerSecond} numbers per second or {numsPerSecond * 8} bytes per second");
                Console.ReadKey();
            }
            else 
            {
                File.AppendAllText("C:\\Users\\Adam\\Documents\\Random\\Hash\\ChaosRunRecord.txt", $"Running Chaos Oracle In Derp Mode {DateTime.Now}" + Environment.NewLine);                
                ulong state = 0;
                using (Stream myOutStream = Console.OpenStandardOutput())
                {                    
                    ulong[] randomData = new ulong[1024];
                    byte[] randomBytes = new byte[1024 * 8];
                    while (true)
                    {
                        for (int i = 0; i < randomData.Length; i++)
                        {
                            randomData[i] = state++;
                        }
                        Buffer.BlockCopy(randomData, 0, randomBytes, 0, randomBytes.Length);
                        myOutStream.Write(randomBytes, 0, randomData.Length * 8);
                    }
                }
            }
        }
    }
}
