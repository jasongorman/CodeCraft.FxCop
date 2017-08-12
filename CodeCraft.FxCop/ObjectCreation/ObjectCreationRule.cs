#define CODE_ANALYSIS
using System.Diagnostics.CodeAnalysis;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.ObjectCreation
{
    public class ObjectCreationRule : MethodRuleBase
    {
        [SuppressMessage("CodeCraft.FxCop", "TT1018:MethodCallRule")]
        public ObjectCreationRule() :
            base(
            "ObjectCreationRule", "CodeCraft.FxCop.ObjectCreation.ObjectCreationRuleMetadata.xml",
            typeof (ObjectCreationRule).Assembly)
        {
        }

        public override void VisitConstruct(Construct construct)
        {
            var createdType = ((MemberBinding) construct.Constructor).BoundMember.DeclaringType;

            if (ShouldBeChecked(createdType))
            {
                string[] resolutionParams = {_method.Name.Name, createdType.Name.Name};
                Problems.Add(
                    new Problem(new Resolution("Method {0} instantiates project class {1}", resolutionParams), construct));
            }
            base.VisitConstruct(construct);
        }
    }
}