using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public interface ICliCommand
    {
        object  Execute(object parameter);
    }
    public interface ICliCommand<TParameter, TResult>: ICliCommand
    {
        TResult Execute(TParameter parameter);
    }
}