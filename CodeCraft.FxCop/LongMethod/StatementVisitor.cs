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
                int startLine = statement.SourceContext.StartLine;
                int endLine = statement.SourceContext.EndLine;

                AddLineNumbers(startLine, endLine);
            }

            base.VisitStatements(statements);
        }

        private void AddLineNumbers(int startLine, int endLine)
        {
            for (int i = startLine; i <= endLine; i++)
            {
                AddLineNumber(i);
            }
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