using System;
using System.Linq;
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
                CheckForFeatureEnvy(method, VisitBodyStatements(method));
            }
            return Problems;
        }

        private MethodInvocationVisitor VisitBodyStatements(Method method)
        {
            var visitor = new MethodInvocationVisitor(method.DeclaringType);
            visitor.VisitStatements(method.Body.Statements);
            return visitor;
        }

        private void CheckForFeatureEnvy(Method method, MethodInvocationVisitor visitor)
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
