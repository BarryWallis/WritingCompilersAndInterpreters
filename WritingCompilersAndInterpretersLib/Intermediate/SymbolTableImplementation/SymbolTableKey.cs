using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

public enum SymbolTableKey
{
    ConstantValue, RoutineCode, RoutineSymbolTable, RoutineIntermediateCode, RoutineParameters, RoutineRoutines, DataValue,
}
