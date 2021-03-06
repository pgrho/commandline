using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class MarkupObject
    {
        internal static readonly MarkupObject[] EmptyObjects = new MarkupObject[0];
        private bool _IsFreezed;

        /// <summary>
        /// <see cref="Parent" />のバッキングストアです。
        /// </summary>
        private MarkupObject _Parent;

        /// <summary>
        /// オブジェクトを所有するオブジェクトを取得します。
        /// </summary>
        public MarkupObject Parent
        {
            get
            {
                return _Parent;
            }
            internal set
            {
                ThrowIfFreezed();
                _Parent = value;
            }
        }

        public void Freeze()
        {
            if (_IsFreezed)
            {
                return;
            }

            OnFreeze();

            _IsFreezed = true;
        }

        protected internal void ThrowIfFreezed()
        {
            if (_IsFreezed)
            {
                throw new InvalidOperationException();
            }
        }

        protected abstract void OnFreeze();
        public static bool IsMetaChar(char @char)
        {
            switch (@char)
            {
                case '\\':
                case '`':
                    return true;
            }
            return false;
        }

        public MarkupObject Clone()
        {
            var other = CreateInstanceCore();
            CopyTo(other);
            return other;
        }

        protected abstract MarkupObject CreateInstanceCore();

        public virtual void CopyTo(MarkupObject other) { }

        public abstract IReadOnlyList<MarkupObject> GetChildren();
    }
}