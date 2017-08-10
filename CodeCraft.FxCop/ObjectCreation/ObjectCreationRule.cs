using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.ObjectCreation
{
    public class ObjectCreationRule : BaseIntrospectionRule
    {
        private readonly List<NodeType> _news = new List<NodeType>();
        private Method _method;

        public ObjectCreationRule() : 
            base("ObjectCreationRule", "CodeCraft.FxCop.ObjectCreation.ObjectCreationRuleMetadata.xml", typeof(ObjectCreationRule).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            _method = member as Method;
            if (_method == null)
            {
                return Problems;
            }
            Visit(_method);
            if (_news.Count > 0)
            {
                string[] resolutionParams = {_method.Name.Name};
                Problems.Add(new Problem(new Resolution("Method {0} instantiates project class", resolutionParams)));
            }

            return base.Check(member);
        }

        public override void VisitConstruct(Construct construct)
        {
            if (IsProjectType(construct.Type) &! IsFactory(_method))
            {
                _news.Add(construct.NodeType);
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
            AssemblyNode assembly = _method.DeclaringType.ContainingAssembly();
            return assembly.Types.Contains(type);
        }
    }
}