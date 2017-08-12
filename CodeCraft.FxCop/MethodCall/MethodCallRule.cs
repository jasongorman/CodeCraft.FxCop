#define CODE_ANALYSIS
using System.Diagnostics.CodeAnalysis;
using CodeCraft.FxCop.ObjectCreation;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MethodCall
{
    public class MethodCallRule : MethodRuleBase
    {
        [SuppressMessage("CodeCraft.FxCop", "TT1018:MethodCallRule")]
        public MethodCallRule() :
            base(
            "MethodCallRule", "CodeCraft.FxCop.MethodCall.MethodCallRuleMetadata.xml",
            typeof (ObjectCreationRule).Assembly)
        {
        }

        public override void VisitMemberBinding(MemberBinding memberBinding)
        {
            var boundMember = memberBinding.BoundMember;
            var callee = boundMember.DeclaringType;
            var caller = _method.DeclaringType;

            var calledMethod = boundMember as Method;

            if (calledMethod == null)
            {
                return;
            }

            if (callee == caller)
            {
                return;
            }

            CheckForProblem(memberBinding, callee);
            base.VisitMemberBinding(memberBinding);
        }

        private void CheckForProblem(MemberBinding memberBinding, TypeNode callee)
        {
            Method boundMethod = memberBinding.BoundMember as Method;

            if (ShouldBeChecked(callee))
            {
                if (!IsSwappable(boundMethod))
                {
                    string[] resolutionParams = {_method.Name.Name, boundMethod.Name.Name};
                    Problems.Add(
                        new Problem(
                            new Resolution("Method {0} invokes non-virtual-or-abstract method {1}", resolutionParams),
                            memberBinding));
                }
            }
        }

        private static bool IsSwappable(Method boundMethod)
        {
            return (boundMethod.IsAbstract || boundMethod.IsVirtual);
        }
    }
}