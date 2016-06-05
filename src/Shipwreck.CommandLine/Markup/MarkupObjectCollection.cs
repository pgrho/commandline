using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class MarkupObjectCollection<T> : Collection<T>
        where T : MarkupObject
    {
        private readonly MarkupObject _Parent;

        protected MarkupObjectCollection(MarkupObject parent)
        {
            _Parent = parent;
        }

        protected override void ClearItems()
        {
            if (_Parent != null)
            {
                foreach (var item in this)
                {
                    item.Parent = null;
                }
            }
            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            if (_Parent != null && item.Parent != null)
            {
                throw new ArgumentException();
            }

            base.InsertItem(index, item);
            if (_Parent != null)
            {
                item.Parent = _Parent;
            }
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            if (_Parent != null)
            {
                item.Parent = null;
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (_Parent != null && item.Parent != null)
            {
                throw new ArgumentException();
            }

            var old = this[index];
            base.SetItem(index, item);

            if (_Parent != null)
            {
                old.Parent = null;
                item.Parent = _Parent;
            }
        }

        internal void Freeze()
        {
            foreach (var item in this)
            {
                this.Freeze();
            }
        }
    }
}