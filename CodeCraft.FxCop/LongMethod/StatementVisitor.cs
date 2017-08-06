using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.LongMethod
{
    public class StatementVisitor : BinaryReadOnlyVisitor
    {
        private readonly List<int> _lineNumbers = new List<int>();

        public int LinesOfCode
        {
            get { return _lineNumbers.Count; }
            
        }

        public override void VisitStatements(StatementCollection statements)
        {
            foreach (var statement in statements)
            {
                AddLineNumber(statement.SourceContext.StartLine);
                AddLineNumber(statement.SourceContext.EndLine);
            }

            base.VisitStatements(statements);
        }

        private void AddLineNumber(int lineNumber)
        {
            if (!_lineNumbers.Contains(lineNumber))
            {
                _lineNumbers.Add(lineNumber);
            }
        }
    }
}