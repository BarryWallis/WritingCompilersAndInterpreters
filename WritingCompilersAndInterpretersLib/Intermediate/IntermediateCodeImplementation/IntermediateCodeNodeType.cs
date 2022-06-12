namespace WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

/// <summary>
/// Node types in the intermediate code parse tree.
/// </summary>
public enum IntermediateCodeNodeType
{
    // Program Structure
    Program, Procedure, Function,

    // Statements
    Compound, Assign, Loop, Test, Call, Parameters,
    If, Select, SelectBranch, SelectConstants, NoOp,

    // Relational Operators
    Eq, Ne, Lt, Le, Gt, Ge, Not,

    // Additive Operators
    Add, Subtract, Or, Negate,

    // Multiplicative Operators
    Multiply, IntegerDivide, FloatDivide, Mod, And,

    // Operands
    Variable, Subscripts, Field,
    IntegerConstant, RealConstant,
    StringConstant, BooleanConstant,

    // Write Parameter
    WriteParm,

}