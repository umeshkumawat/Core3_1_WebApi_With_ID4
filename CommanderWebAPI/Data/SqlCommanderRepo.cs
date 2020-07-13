using CommanderWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanderWebAPI.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext _commanderContext;

        public SqlCommanderRepo(CommanderContext commanderContext )
        {
            _commanderContext = commanderContext;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException();

            _commanderContext.Commands.Add(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _commanderContext.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _commanderContext.Commands.FirstOrDefault(c => c.Id == id);
        }

        public bool SaveChanges()
        {
            return (_commanderContext.SaveChanges() >= 0);
        }

        public void UpdateCommand(Command cmd)
        {
            _commanderContext.Update(cmd);
        }
    }
}
