using rpa_fotografia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpa_fotografia.DataAccess
{
    public class ClienteService
    {
        public readonly Database _database;

        public ClienteService(Database database)
        {
            _database = database;
        }

        public int AdicionarCliente(Cliente cliente)
        {
            int linhasAfetadas = _database.InsertOrUpdateCliente(cliente);
            return linhasAfetadas;
        }
    }
}
