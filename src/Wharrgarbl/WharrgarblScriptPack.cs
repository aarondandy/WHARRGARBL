namespace Wharrgarbl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ScriptCs.Contracts;

    /// <inheritdoc/>
    public class WharrgarblScriptPack : IScriptPack
    {
        /// <inheritdoc/>
        public IScriptPackContext GetContext()
        {
            return new WharrgarblScriptPackContext();
        }

        /// <inheritdoc/>
        public void Initialize(IScriptPackSession session)
        {
            var namespaceUsings = new[]
            {
                "Wharrgarbl.CoreExtensions",
                "Wharrgarbl.Lifetimes"
            };
            var functionUsings = new[]
            {
                "Wharrgarbl.Functions.EnumFn",
                "Wharrgarbl.Functions.EnvFn",
                "Wharrgarbl.Functions.Fn"
            };
            foreach (var @namespace in namespaceUsings.Concat(functionUsings))
            {
                session.ImportNamespace(@namespace);
            }
        }

        /// <inheritdoc/>
        public void Terminate()
        {
        }
    }
}
