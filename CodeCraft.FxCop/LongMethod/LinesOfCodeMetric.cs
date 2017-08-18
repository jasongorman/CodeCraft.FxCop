using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.LongMethod
{
    internal class LinesOfCodeMetric : IMetric
    {
        private readonly List<OpCode> _nonSourceCodes = new List<OpCode>()
        {
            OpCode.Nop,
            OpCode._Try,
            OpCode.Leave_S,
            OpCode.Ret
        };

        public int Calculate(Node method)
        {
            return ((Method)method).Instructions
                .Where(IsFromSourceCode)
                .Select(i => i.SourceContext.StartLine)
                .Distinct()
                .Count();
        }

        private bool IsFromSourceCode(Instruction instruction)
        {
            return !_nonSourceCodes.Contains(instruction.OpCode) 
                && instruction.SourceContext.FileName != null;
        }
    }
}