using Shipwreck.CommandLine.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public abstract class CommandsHost<TParameter, TResult> : ICliCommandsHost<TParameter, TResult>
    {
        private HelpWriter _HelpWriter;

        public abstract TResult Execute(TParameter parameter);

        object ICliCommand.Execute(object parameter)
            => Execute((TParameter)parameter);

        public HelpWriter HelpWriter
        {
            get
            {
                return _HelpWriter ?? (_HelpWriter = new ConsoleHelpWriter());
            }
            set
            {
                _HelpWriter = value;
            }
        }
    }
}