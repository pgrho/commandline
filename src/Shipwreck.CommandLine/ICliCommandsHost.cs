using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public interface ICliCommandsHost<TParameter, TResult> : ICliCommand<TParameter, TResult>
    {
    }
}