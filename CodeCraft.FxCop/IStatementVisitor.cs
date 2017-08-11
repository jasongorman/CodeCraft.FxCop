using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop
{
    public interface IStatementVisitor
    {
        void VisitStatements(StatementCollection statements);
        List<TypeNode> EnviedTypes { get; }
    }
}