using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop
{
    internal interface IMetric
    {
        int Calculate(Node node);
    }
}