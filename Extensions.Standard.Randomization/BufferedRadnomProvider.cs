using System;

namespace Extensions.Standard.Randomization
{
    /// <summary>Preserved for binary compatibility. Use <see cref="BufferedRandomProvider"/> instead (fixed spelling).</summary>
    [Obsolete("Use BufferedRandomProvider instead (typo in original name fixed). This class will be removed in a future major version.")]
    public sealed class BufferedRadnomProvider : BufferedRandomProvider
    {
        /// <inheritdoc/>
        public BufferedRadnomProvider(int bufferSize) : base(bufferSize) { }
    }
}
