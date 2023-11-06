using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnderPi.Random.Test
{
    [Serializable]
    public class ThirdOrderGorilla : IIncrementalRandomTest
    {
        private IRandomEngine _engine;
        private UInt32[][] _counts;
        private int _wordSize;
        private BitByBitWords _words;
        private int _counter;
        private UInt64 _iterationsPerformed;
        private UInt64 _minimumIterationsForResult;
        private double[] _pValues;
        private TestResult[] _testResults;
        private double _aggregatePValue;
        private TestResult _aggregateTestResult;
        private TestResult _overallTestResult;
        private int _cellCount;

        private static readonly ulong[] constants = { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831, 493103506155265287, 689370800073552075, 2303624318106399810, 9496165304756913731, 12035424005045793793, 6685197515345832855, 2656657354890179337, 9720106778309819524, 14715118401985547901, 14918721632492036331, 14916780797981785668, 6653603225583465284, 11459605610878049781, 15367375858701530787, 11689533043359152663, 15247888379347215110, 16122983862132142910, 12356767314225484987, 13581698193155494064, 15822743625088846253, 13266281975252868687, 11553465162911432431, 4972399138750820749, 5013820095768955290, 2435012454098703131, 7794617134289375954, 9491423185294945758, 5784059636265243100, 4962101747692318209, 2412233113776848279, 5474733583693578266, 10378251128189562191, 11779690937116173544, 4113455014468819836, 11181584313450578738, 15287848078269490984, 9383066286683014594, 10837563506325318076, 5104151911441924943, 4114395767014852192, 6635459657931808133, 11553165155891861824, 9573537094354532727, 4615691730115113604, 15516039487186883944, 737422353624691682, 6945706647696304751, 5691229456008476387, 1308515051450429517, 15440020319491276741, 8555343322742461026, 7481302235789478987, 4958482254342815994, 16551925689093168667 };

        public TestResult Result => _overallTestResult;

        public int TestsPassed { get { return _testResults.Where(x => x != TestResult.Fail).Count(); } }

        public ThirdOrderGorilla(int wordSize)
        {
            _wordSize = wordSize;
            _cellCount = Convert.ToInt32(Math.Pow(2, _wordSize));
            _minimumIterationsForResult = Convert.ToUInt64(5 * _cellCount);
        }

        public string GetDetailedResult()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Third Order Gorilla Test with word size = {_wordSize}");
            for (int i = 0; i < 64; i++)
            {
                sb.AppendLine($"p-value for bit {i} = {_pValues[i]} with test result {_testResults[i]}");
            }
            sb.AppendLine($"aggregate p-value {_aggregatePValue}");
            sb.AppendLine($"Result of {_overallTestResult}");
            return sb.ToString();
        }

        private void GetTestResults()
        {
            for (int i = 0; i < _pValues.Length; i++)
            {
                _testResults[i] = TestHelper.GetTestResultFromPValue(_pValues[i]);
            }
        }

        private void GetChiSquaredPValues()
        {
            double expectedValue = _iterationsPerformed / (double)_cellCount;
            double[] chiSquaredValues = new double[64];
            //need an array of worst

            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < _counts[i].Length; j++)
                {
                    var difference = _counts[i][j] - expectedValue;
                    chiSquaredValues[i] += difference * difference;
                }
                _pValues[i] = TestHelper.ChiSquaredPValue(_cellCount - 1, chiSquaredValues[i] / expectedValue);
            }
            return;
        }

        /// <summary>
        /// Treats the random engine like a hash by directly seeding it, then asking for the output.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private ulong Hash(ulong x)
        {
            _engine.Seed(x);
            return _engine.Nextulong();
        }

        public void Process(UInt64 randomNumber)
        {
            ulong result = Hash(randomNumber) ^ Hash(randomNumber ^ constants[0]) ^ Hash(randomNumber ^ constants[1]) ^ Hash(randomNumber ^ constants[2]) ^ 
                Hash(randomNumber ^ constants[0] ^ constants[1]) ^ Hash(randomNumber ^ constants[1] ^ constants[2]) ^ Hash(randomNumber ^ constants[0] ^ constants[2])
                ^ Hash(randomNumber ^ constants[0] ^ constants[1] ^ constants[2]);
            _words.IncrementState(randomNumber);
            _counter++;
            if (_counter >= _wordSize)
            {
                RunOneIteration();
                _counter = 0;
                _words.Reset();
            }
        }

        private void RunOneIteration()
        {
            for (int j = 0; j < 64; j++)
            {
                _counts[j][_words[j]]++;
            }
            _iterationsPerformed++;
        }

        public void CalculateResult(bool detailed)
        {
            if (_iterationsPerformed < _minimumIterationsForResult)
            {
                return;
            }

            GetChiSquaredPValues();
            _aggregatePValue = ChiSquaredTest.ChiSquaredForPValues(_pValues);
            GetTestResults();
            _aggregateTestResult = TestHelper.GetTestResultFromPValue(_aggregatePValue);

            TestResult minimumTestResult = TestHelper.ReturnLowestConclusiveResult(_testResults);
            _overallTestResult = TestHelper.ReturnLowestConclusiveResult(minimumTestResult, _aggregateTestResult);

            return;
        }

        public void Initialize(IRandomEngine engine)
        {
            _engine = engine.DeepCopy();
            _counts = new uint[64][];
            _pValues = new double[64];
            _testResults = new TestResult[64];
            for (int i = 0; i < 64; i++)
            {
                _counts[i] = new uint[_cellCount];
            }
            _overallTestResult = TestResult.Inconclusive;
            _words = new BitByBitWords(_wordSize);
        }

        public override string ToString()
        {
            return $"Third Gorilla Test, Word length {_wordSize} bits";
        }

        public string GetFailureDescriptions()
        {
            StringBuilder sb = new StringBuilder();
            if (Result == TestResult.Fail)
            {
                sb.AppendLine($"Third Order Gorilla Test with word size = {_wordSize}");
                for (int i = 0; i < 64; i++)
                {
                    if (_testResults[i] == TestResult.Fail)
                    {
                        sb.AppendLine($"p-value for bit {i} = {_pValues[i]} with test result {_testResults[i]}");
                    }
                }
                if (_overallTestResult == TestResult.Fail)
                {
                    sb.AppendLine($"Aggregate p-value {_aggregatePValue}");
                    sb.AppendLine($"Result of {_overallTestResult}");
                }
            }
            return sb.ToString();
        }

        public TestType GetTestType()
        {
            return TestType.ThirdOrderGorilla;
        }
    }
}
