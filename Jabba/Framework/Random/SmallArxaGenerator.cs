using EnderPi.SystemE;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;

namespace EnderPi.Random
{
    public class SmallArxaGenerator
    {
        private UInt16 _stateOne;
        private UInt16 _stateTwo;
        private UInt16 _additiveConstant;
        private int _rotateConstant;
        private int _xsrConstant;



        public UInt16 StateOne { set { _stateOne = value; } get { return _stateOne; } }
        public UInt16 StateTwo { set { _stateTwo = value; } get { return _stateTwo; } }

        public int XsrConstant { set { _xsrConstant = value; } get { return _xsrConstant; } }
        public UInt16 AdditiveConstant { set { _additiveConstant = value; } get { return _additiveConstant; } }
        public int RotateConstant { set { _rotateConstant = value; } get { return _rotateConstant; } }


        public UInt16 Next()
        {
            StateOne = (UInt16)(StateOne ^ (StateOne >> XsrConstant));
            StateOne += StateTwo;            
            StateTwo += AdditiveConstant;
            StateOne = BitHelper.RotateLeft(StateOne, RotateConstant);
            return StateOne;
        }

        public void Reset()
        {
            StateOne = StateTwo= 0;
        }

    }
}
