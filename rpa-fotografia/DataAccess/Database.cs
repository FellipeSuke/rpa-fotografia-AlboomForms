using MySql.Data.MySqlClient;
using rpa_fotografia.Models;
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
                    string query = @"INSERT INTO clientes 
                        (data, NomeFormulario, Link, Nome, SobreNome, CPF, Email, WhatsApp, CEP, Rua, Bairro, Cidade, Estado, Pais, Numero, Complemento) 
                        VALUES (@Data, @NomeFormulario, @Link, @Nome, @SobreNome, @CPF, @Email, @WhatsApp, @CEP, @Rua, @Bairro, @Cidade, @Estado, @Pais, @Numero, @Complemento)
                        ON DUPLICATE KEY UPDATE
                        data = @Data,
                        NomeFormulario = @NomeFormulario,
                        Link = @Link,
                        Nome = @Nome,
                        SobreNome = @SobreNome,
                        CPF = @CPF,
                        WhatsApp = @WhatsApp,
                        CEP = @CEP,
                        Rua = @Rua,
                        Bairro = @Bairro,
                        Cidade = @Cidade,
                        Estado = @Estado,
                        Pais = @Pais,
                        Numero = @Numero,
                        Complemento = @Complemento";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Data", cliente.Data);
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
                        command.Parameters.AddWithValue("@Estado", cliente.Estado);
                        command.Parameters.AddWithValue("@Pais", cliente.Pais);
                        command.Parameters.AddWithValue("@Numero", cliente.Numero);
                        command.Parameters.AddWithValue("@Complemento", cliente.Complemento);

                        return command.ExecuteNonQuery();
                    }
                }

            }
            catch
            {
                return 0;
            }
        }

    }
}
