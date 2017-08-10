using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MethodComplexity
{
    internal class ComplexityMetrics
    {
        private readonly List<OpCode> _conditionals = new List<OpCode>
        {
            OpCode.Brfalse_S,
            OpCode.Brtrue_S,
            OpCode.Beq_S,
            OpCode.Bge_S,
            OpCode.Bgt_S,
            OpCode.Ble_S,
            OpCode.Blt_S,
            OpCode.Bne_Un_S,
            OpCode.Bge_Un_S,
            OpCode.Bgt_Un_S,
            OpCode.Ble_Un_S,
            OpCode.Blt_Un_S,
            OpCode.Brfalse,
            OpCode.Brtrue,
            OpCode.Beq,
            OpCode.Bge,
            OpCode.Bgt,
            OpCode.Ble,
            OpCode.Blt,
            OpCode.Bne_Un,
            OpCode.Bge_Un,
            OpCode.Bgt_Un,
            OpCode.Ble_Un,
            OpCode.Blt_Un
        };

        internal int CalculateComplexity(Method method)
        {
            return method.Instructions.Sum(i => Complexity(i)) + 1;
        }

        private int Complexity(Instruction instruction)
        {
            if (IsSingleBranch(instruction))
            {
                return 1;
            }
            if (IsSwitch(instruction))
            {
                return SwitchComplexity(instruction);
            }
            return 0;
        }

        private bool IsSingleBranch(Instruction instruction)
        {
            return IsConditional(instruction) || IsCatchOrFault(instruction);
        }

        private bool IsCatchOrFault(Instruction instruction)
        {
            return instruction.OpCode == OpCode._Catch || instruction.OpCode == OpCode._Fault;
        }

        private static bool IsSwitch(Instruction i)
        {
            return i.OpCode == OpCode.Switch;
        }

        private int SwitchComplexity(Instruction current)
        {
            var targets = (IEnumerable<int>) current.Value;
            return new HashSet<int>(targets).Count;
        }

        private bool IsConditional(Instruction instruction)
        {
            return _conditionals.Contains(instruction.OpCode);
        }
    }
}