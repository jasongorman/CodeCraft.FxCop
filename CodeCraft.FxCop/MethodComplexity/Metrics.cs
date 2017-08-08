using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MethodComplexity
{
    internal class Metrics
    {
        internal int CalculateComplexity(Method method)
        {
            var num = 1;
            var enumerator = method.Instructions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var opCode = current.OpCode;
                switch (opCode)
                {
                    case OpCode.Brfalse_S:
                    case OpCode.Brtrue_S:
                    case OpCode.Beq_S:
                    case OpCode.Bge_S:
                    case OpCode.Bgt_S:
                    case OpCode.Ble_S:
                    case OpCode.Blt_S:
                    case OpCode.Bne_Un_S:
                    case OpCode.Bge_Un_S:
                    case OpCode.Bgt_Un_S:
                    case OpCode.Ble_Un_S:
                    case OpCode.Blt_Un_S:
                    case OpCode.Brfalse:
                    case OpCode.Brtrue:
                    case OpCode.Beq:
                    case OpCode.Bge:
                    case OpCode.Bgt:
                    case OpCode.Ble:
                    case OpCode.Blt:
                    case OpCode.Bne_Un:
                    case OpCode.Bge_Un:
                    case OpCode.Bgt_Un:
                    case OpCode.Ble_Un:
                    case OpCode.Blt_Un:
                        break;
                    case OpCode.Br:
                        continue;
                    case OpCode.Switch:
                    {
                        var enumerable = (IEnumerable<int>) current.Value;
                        var hashSet = new HashSet<int>();
                        foreach (var current2 in enumerable)
                        {
                            hashSet.Add(current2);
                        }
                        num += hashSet.Count;
                        continue;
                    }
                    default:
                        switch (opCode)
                        {
                            case OpCode._Catch:
                            case OpCode._Fault:
                                break;
                            case OpCode._Finally:
                                continue;
                            default:
                                continue;
                        }
                        break;
                }
                num++;
            }
            return num;
        }

        internal int CalculateLines(Method method)
        {
            SortedSet<int> set = new SortedSet<int>();
            MetadataCollection<Instruction>.Enumerator enumerator = method.Instructions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Instruction current = enumerator.Current;
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