using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.ClientInterface
{
    public class ClientInterfaceRule : BaseIntrospectionRule
    {
        private readonly Dictionary<TypeNode, List<Method>> membersUsed = new Dictionary<TypeNode, List<Method>>();
        private readonly Dictionary<TypeNode, List<Method>> membersExposed = new Dictionary<TypeNode, List<Method>>();
        private TypeNode _caller;

        public ClientInterfaceRule() : base("ClientInterfaceRule", "CodeCraft.FxCop.ClientInterface.ClientInterfaceRuleMetadata.xml", typeof(ClientInterfaceRule).Assembly)
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
            _caller = type;

            foreach (Member member in type.Members)
            {
                Method method = member as Method;
                if (method != null)
                {
                    VisitStatements(method.Body.Statements);
                    
                }
            }
            CheckIfRuleBroken();
            return Problems;
        }

        private void CheckIfRuleBroken()
        {
            foreach (TypeNode callee in membersUsed.Keys)
            {
                if (!IsClientSpecific(callee))
                {
                    string[] resolutionParams = {callee.Name.Name, _caller.Name.Name};
                    Problems.Add(new Problem(new Resolution("The interface of {0} exposes more features than {1} uses", resolutionParams),callee));
                }
            }
        }

        private bool IsClientSpecific(TypeNode callee)
        {
            HashSet<Member> used = new HashSet<Member>(membersUsed[callee]);
            HashSet<Member> exposed = new HashSet<Member>(membersExposed[callee]);
            return exposed.IsSubsetOf(used);
        }

        public override void VisitMemberBinding(MemberBinding binding)
        {
            var boundMember = binding.BoundMember;
            TypeNode callee = boundMember.DeclaringType;

            if (_caller.DeclaringModule.Types.Contains(callee) && IsCountedMember(callee, boundMember))
            {
                AddBoundMember(callee, boundMember);
            }
        }

        private bool IsCountedMember(TypeNode callee, Member boundMember)
        {
            return _caller != callee 
                && !_caller.IsDerivedFrom(callee) 
                && boundMember.NodeType != NodeType.InstanceInitializer;
        }

        private void AddBoundMember(TypeNode callee, Member boundMember)
        {
            if (!membersUsed.ContainsKey(callee))
            {
                membersUsed.Add(callee, new List<Method>());
                membersExposed.Add(callee, GetExposedMembers(callee));
            }
            membersUsed[callee].Add(boundMember as Method);
        }

        private List<Method> GetExposedMembers(TypeNode callee)
        {
            return callee.Members.OfType<Method>().Where(m => !m.IsPrivate && m.NodeType != NodeType.InstanceInitializer).ToList();
        }
    }
}