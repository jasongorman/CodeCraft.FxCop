using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.ObjectCreation
{
    public class ObjectCreationRule : BaseIntrospectionRule
    {
        private Method _method;

        public ObjectCreationRule() :
            base(
            "ObjectCreationRule", "CodeCraft.FxCop.ObjectCreation.ObjectCreationRuleMetadata.xml",
            typeof (ObjectCreationRule).Assembly)
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

        public override void VisitConstruct(Construct construct)
        {
            var createdType = ((MemberBinding) construct.Constructor).BoundMember.DeclaringType;

            if (IsProjectType(createdType) & !IsFactory(_method))
            {
                    string[] resolutionParams = {_method.Name.Name, createdType.Name.Name};
                    Problems.Add(
                        new Problem(new Resolution("Method {0} instantiates project class {1}", resolutionParams), construct));
            }
            base.VisitConstruct(construct);
        }

        private bool IsFactory(Method method)
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

        private bool IsProjectType(TypeNode type)
        {
            return _method.DeclaringType.DeclaringModule.Types.Contains(type);
        }
    }
}