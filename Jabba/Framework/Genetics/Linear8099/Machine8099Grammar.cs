
namespace EnderPi.Genetics.Linear8099
{
    /// <summary>
    /// Just a central place to list the string form of the 8099 instruction set.
    /// </summary>
    public static class Machine8099Grammar
    {
        public const string Move = "MOV";
        public const string Nope = "NOP";
        public const string Add = "ADD";
        public const string Subtract = "SUB";
        public const string Multiply = "MUL";
        public const string Divide = "DIV";
        public const string Remainder = "REM";
        public const string And = "AND";
        public const string Not = "NOT";
        public const string Or = "OR";
        public const string Xor = "XOR";
        public const string ShiftLeft = "SHL";
        public const string ShiftRight = "SHR";
        public const string RotateRight = "ROR";
        public const string RotateLeft = "ROL";
        public const string XorShiftRight = "XSR";
        public const string RotateMultiply = "RMU";
        public const string Loop = "LOP";
    }
}
