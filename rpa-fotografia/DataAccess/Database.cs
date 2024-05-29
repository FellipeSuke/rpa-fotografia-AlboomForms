using MySql.Data.MySqlClient;
using rpa_fotografia.Models;
using rpa_fotografia.Utils;
namespace rpa_fotografia.DataAccess
{
    public class Database
    {
        public string? connectionString;

        public Database(ConnectionStringsJ host)
        {
            connectionString = host.DefaultConnection;
        }

        public int InsertOrUpdateCliente(Cliente cliente)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Logger.Log("Conexão com o banco " + connection.State.ToString());

                    var command = new MySqlCommand(
                               "INSERT INTO clientes (data, NomeFormulario, Link, Nome, SobreNome, CPF, Email, WhatsApp, CEP, Rua, Bairro, Cidade, Estado, Pais, Numero, Complemento, importado_crm) " +
                               "VALUES (@data, @NomeFormulario, @Link, @Nome, @SobreNome, @CPF, @Email, @WhatsApp, @CEP, @Rua, @Bairro, @Cidade, @Estado, @Pais, @Numero, @Complemento, 0) " +
                               "ON DUPLICATE KEY UPDATE " +
                               "data = VALUES(data), NomeFormulario = VALUES(NomeFormulario), Link = VALUES(Link), Nome = VALUES(Nome), SobreNome = VALUES(SobreNome), CPF = VALUES(CPF), " +
                               "Email = VALUES(Email), WhatsApp = VALUES(WhatsApp), CEP = VALUES(CEP), Rua = VALUES(Rua), Bairro = VALUES(Bairro), Cidade = VALUES(Cidade), Estado = VALUES(Estado), " +
                               "Pais = VALUES(Pais), Numero = VALUES(Numero), Complemento = VALUES(Complemento), importado_crm = VALUES(importado_crm)",
                               connection);

                    command.Parameters.AddWithValue("@data", cliente.Data);
                    command.Parameters.AddWithValue("@NomeFormulario", cliente.NomeFormulario);
                    command.Parameters.AddWithValue("@Link", cliente.Link);
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@SobreNome", cliente.SobreNome);
                    command.Parameters.AddWithValue("@CPF", cliente.CPF);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@WhatsApp", cliente.WhatsApp);
                    command.Parameters.AddWithValue("@CEP", cliente.CEP);
                    command.Parameters.AddWithValue("@Rua", cliente.Rua);
                    command.Parameters.AddWithValue("@Bairro", cliente.Bairro);
                    command.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                    command.Parameters.AddWithValue("@Estado", cliente.Estado); // Nome completo do estado
                    command.Parameters.AddWithValue("@Pais", cliente.Pais);
                    command.Parameters.AddWithValue("@Numero", cliente.Numero);
                    command.Parameters.AddWithValue("@Complemento", cliente.Complemento);
                    Thread.Sleep(200);

                    int linhasAfetadas = command.ExecuteNonQuery();
                        Logger.Log(linhasAfetadas + " Linhas afetadas na execução da query");
                        return linhasAfetadas;
                    

                    connection.Close();

                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                return 0;
            }
        }

    }
}
