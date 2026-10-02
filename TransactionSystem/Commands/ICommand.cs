using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionSystem.UI.Commands
{
    public interface ICommand
    {
        CommandResult Execute(Context context);
    }
}
