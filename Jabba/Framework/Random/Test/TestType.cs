using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Random.Test
{
    public enum TestType
    {
        ZeroTest = 0,
        GcdTest = 1,
        LinearCorrelation = 2,
        Gorilla = 3,
        LinearHash = 4,
        DifferentialHash = 5,
        LawOfIteratedLogarithm = 6,
        LinearDifferentialHash = 7,
        DifferentialPrfHash = 8,
        ZeroHash = 9,
        DifferentialHashComplex = 10,
        DifferentialPrfComplex = 11,
        LowerFourBitsTest = 12,
        AllBitsTest = 13,
        DifferentialUniformity = 14,
        Monkey = 15,
        DifferentialSecondOrder=16,
        ThirdOrderGorilla=17,
        MaurerBytewise=18,
        GapByteTest=19
    }
}
