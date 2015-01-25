namespace Wharrgarbl.Lifetimes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A scoped environment variable lifetime implementing the <see cref="System.IDisposable">IDisposable</see> pattern.
    /// </summary>
    public class EnvVarLifetime : IDisposable
    {
        private EnvVarLifetime(string variable, string newValue, string originalValue, EnvironmentVariableTarget target) 
        {
            Contract.Requires(!string.IsNullOrEmpty(variable));

            Variable = variable;
            NewValue = newValue;
            OriginalValue = originalValue;
            Target = target;
        }

        /// <inheritdoc/>
        ~EnvVarLifetime()
        {
            Restore();
        }

        public string Variable { get; private set; }

        public string NewValue { get; private set; }

        public string OriginalValue { get; private set; }

        public EnvironmentVariableTarget Target { get; private set; }

        public static EnvVarLifetime Set(string variable, string newValue)
        {
            Contract.Requires(!string.IsNullOrEmpty(variable));
            return Set(variable, newValue, EnvironmentVariableTarget.Process);
        }

        public static EnvVarLifetime Set(string variable, string newValue, EnvironmentVariableTarget target)
        {
            if (string.IsNullOrEmpty(variable)) throw new ArgumentNullException("variable");
            Contract.Ensures(Contract.Result<EnvVarLifetime>() != null);

            var originalValue = Environment.GetEnvironmentVariable(variable, target);
            Environment.SetEnvironmentVariable(variable, newValue);
            return new EnvVarLifetime(variable, newValue, originalValue, target);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Restore();
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(Variable != null);
        }

        private void Restore()
        {
            Environment.SetEnvironmentVariable(Variable, OriginalValue, Target);
        }
    }
}
