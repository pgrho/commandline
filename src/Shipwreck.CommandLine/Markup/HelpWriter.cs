using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class HelpWriter
    {
        public bool IsInList { get; protected set; }

        public virtual void ResetMessageType()
        {
        }

        public virtual void EnterWarning()
        {
        }

        public virtual void EnterError()
        {
        }

        public virtual void EnterList()
            => IsInList = true;

        public virtual void ExitList()
            => IsInList = false;

        public abstract void WriteListBullet();

        public abstract void Write(string message);

        public abstract void WriteCode(string message);

        public abstract void WriteLine();

        public abstract void Flush();
    }

}