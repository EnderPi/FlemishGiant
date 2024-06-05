using EnderPi.Cryptography;
using EnderPi.Random.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RngGenetics.Helper
{
    public static class UIHelperMethods
    {
        public static void AddTestsToCheckedListBoxes(CheckedListBox box)
        {
            box.Items.Add(new ZeroTest(), false);
            box.Items.Add(new GcdTest(), false);
            box.Items.Add(new GorillaTest(7), false);
            box.Items.Add(new GorillaTest(17), false);
            box.Items.Add(new LinearSerialTest(), false);
            box.Items.Add(new LawOfIteratedLogarithmTest(), false);
            box.Items.Add(new LowerFourBitsTest(), false);
            box.Items.Add(new MaurerByteTestIncremental(), false);
            box.Items.Add(new GapByteTest(), false);
            box.Items.Add(new EachBitTest(), false);
            box.Items.Add(new LinearHashTest(), false);
            box.Items.Add(new DifferentialTest(), false);
            box.Items.Add(new DifferentialSecondOrderTest(), false);
            box.Items.Add(new ThirdOrderGorilla(7), false);
            box.Items.Add(new LinearDifferentialTest(), false);
            box.Items.Add(new DifferentialPseudoRandomFunctionTest(), false);
            box.Items.Add(new DifferentialPrfThreeTest(), false);
            box.Items.Add(new ZeroHashTest(), false);
            ulong[] masks = new ulong[64];
            PseudorandomPermutation p = new PseudorandomPermutation();
            for (int i = 0; i < masks.Length; i++)
            {
                masks[i] = p.F((ulong)i);
            }
            box.Items.Add(new DifferentialTest(masks), false);
            box.Items.Add(new DifferentialPseudoRandomFunctionTest(masks), false);
        }

    }
}
