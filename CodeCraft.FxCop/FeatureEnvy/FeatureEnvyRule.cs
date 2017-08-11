using System;
using System.Linq;
using CodeCraft.FxCop.LongMethod;
using CodeCraft.FxCop.MethodComplexity;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.FeatureEnvy
{
    public class FeatureEnvyRule : BaseIntrospectionRule
    {
        public FeatureEnvyRule()
            : base(
                "FeatureEnvyRule", "CodeCraft.FxCop.FeatureEnvy.FeatureEnvyRuleMetadata",
                typeof (FeatureEnvyRule).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            if (member.NodeType == NodeType.Method)
            {
                var method = member as Method;
                CheckFeatureEnvy(method, VisitBodyStatements(method));
            }

            return Problems;
        }

        private IStatementVisitor VisitBodyStatements(Method method)
        {
            var visitor = CreateVisitor(method);
            visitor.VisitStatements(method.Body.Statements);
            return visitor;
        }

        private IStatementVisitor CreateVisitor(Method method)
        {
            return new MethodCallVisitor(method.DeclaringType);
        }

        private void CheckFeatureEnvy(Method method, IStatementVisitor visitor)
        {
            var enviedTypes = visitor.EnviedTypes;
            if (enviedTypes.Count > 0)
            {
                var enviedTypeNames = String.Join(",", enviedTypes.Select(t => t.Name.Name).ToArray());
                Problems.Add(
                    new Problem(new Resolution("Detected Feature Envy in {0} for types {1}",
                        new[] {method.FullName, enviedTypeNames})));
            }
        }
    }
}
