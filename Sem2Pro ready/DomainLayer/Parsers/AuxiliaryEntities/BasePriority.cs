namespace Infrastructure
{
    public enum BasePriority
    {
        Min = 1,
        Addition = 1,
        Subtraction = 1,

        Modulo = 2,

        Multiplication = 3,
        Division = 3,
        IntegerDivision = 3,

        ImplicitMultiplication = 4,
        Argument = 4,

        Power = 5,

        BracketDelta = 6,
        AbsDelta = 6
        // ?
        // ?
    }
}