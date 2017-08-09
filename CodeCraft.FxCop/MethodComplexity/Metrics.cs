using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MethodComplexity
{
    internal class Metrics
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
            if (IsConditional(instruction) || IsCatchOrFault(instruction))
            {
                return 1;
            }
            if (IsSwitch(instruction))
            {
                return SwitchComplexity(instruction);
            }
            return 0;
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

        internal int CalculateLines(Method method)
        {
            var set = new SortedSet<int>();
            var enumerator = method.Instructions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (current.OpCode != OpCode.Nop && current.OpCode != OpCode._Try && current.OpCode != OpCode.Leave_S &&
                    current.OpCode != OpCode.Ret && current.SourceContext.FileName != null)
                {
                    set.Add(current.SourceContext.StartLine);
                }
            }
            return set.Count;
        }
    }
}