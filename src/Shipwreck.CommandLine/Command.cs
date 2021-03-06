using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public abstract class Command<TParameter, TResult> : ICliCommand<TParameter, TResult>
    {
        public abstract TResult Execute(TParameter parameter);

        object ICliCommand.Execute(object parameter)
            => Execute((TParameter)parameter);
    }
}