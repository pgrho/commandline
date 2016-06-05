using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class MarkupLineBreak : MarkupInline
    {
        protected override void OnFreeze()
        { }

        public new MarkupLineBreak Clone()
            => (MarkupLineBreak)base.Clone();

        /// <inheritdoc />
        protected override MarkupObject CreateInstanceCore()
            => new MarkupLineBreak();
    }
}