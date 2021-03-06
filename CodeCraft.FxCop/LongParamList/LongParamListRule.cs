﻿using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.LongParamList
{
    public class LongParamListRule : BaseIntrospectionRule
    {
        public LongParamListRule()
            : base(
                "LongParamListRule", "CodeCraft.FxCop.LongParamList.LongParamListRuleMetadata",
                typeof (LongParamListRule).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            Method method = member as Method;

            if (method == null)
            {
                return Problems;
            }

            CheckParamsLength(method);
            return this.Problems;
        }

        private void CheckParamsLength(Method method)
        {
            if (method.Parameters.Count > 3)
            {
                string[] resolutionParams = {method.FullName};
                Problems.Add(new Problem(new Resolution("Method {0} has too many parameters", resolutionParams)));
            }
        }
    }
}
