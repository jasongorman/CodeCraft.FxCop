using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop
{
    public interface IMetric
    {
        int Calculate(Node node);
    }
}