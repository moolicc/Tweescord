using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tweescord.Commands
{
    abstract class CommandBase
    {
        public abstract string Name { get; }
        public abstract string Usage { get; }
        public abstract string Description { get; }

        public abstract void Execute(Discord discord, params string[] parameters);
    }
}
