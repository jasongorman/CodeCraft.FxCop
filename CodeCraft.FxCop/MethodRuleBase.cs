using System.Reflection;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop
{
    public abstract class MethodRuleBase : BaseIntrospectionRule
    {
        protected Method _method;

        protected MethodRuleBase(string name, string resourceName, Assembly resourceAssembly) : base(name, resourceName, resourceAssembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            _method = member as Method;

            if (_method == null)
                return Problems;

            VisitStatements(_method.Body.Statements);
            return Problems;
        }

        protected virtual bool IsFactory(Method method)
        {
            return IsFactoryName(method.Name.Name)
                   && ReturnsAbstractType(method);
        }

        private bool ReturnsAbstractType(Method method)
        {
            return (method.ReturnType != FrameworkTypes.Void && method.ReturnType.IsAbstract);
        }

        private bool IsFactoryName(string name)
        {
            return (name.StartsWith("Create") || name.StartsWith("Build"));
        }

        protected virtual bool IsProjectType(TypeNode type)
        {
            return _method.DeclaringType.DeclaringModule.Types.Contains(type);
        }

        protected virtual bool ShouldBeChecked(TypeNode callee)
        {
            return IsProjectType(callee) && !IsFactory(_method);
        }
    }
}
