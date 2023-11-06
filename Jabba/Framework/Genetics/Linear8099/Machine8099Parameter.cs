using System;
using System.Collections.Generic;
using System.Text;

namespace EnderPi.Genetics.Linear8099
{
    public class Machine8099Parameter
    {
        private string _text;
        private bool _isRegister;
        private bool _isUlong;
        private bool _isInt;
        private Machine8099Registers _register;
        private ulong _ulongConstant;
        private int _intConstant;

        public bool IsRegister => _isRegister;
        public bool IsUlong => _isUlong;
        public bool IsInt => _isInt;
        public Machine8099Registers Register => _register;
        public ulong UlongConstant => _ulongConstant;
        public int IntConstant => _intConstant;


        public Machine8099Parameter(string s)
        {
            _text = s;

            _isUlong = ulong.TryParse(_text, out _ulongConstant);
            if (!_isUlong && Enum.TryParse(typeof(Machine8099Registers), _text, true, out object reg) && Enum.IsDefined(typeof(Machine8099Registers), reg))
            {
                _isRegister = true;
                _register = (Machine8099Registers)reg;
            }            
            if (_isUlong && _ulongConstant < int.MaxValue)
            {
                _isInt = true;
                _intConstant = Convert.ToInt32(_ulongConstant);
            }
        }

        

    }
}
