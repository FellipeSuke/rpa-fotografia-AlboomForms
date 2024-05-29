using rpa_fotografia.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpa_fotografia.Utils
{
    public class ConvertClass
    {
        public static string ConvertToDateTime(string input)
        {
            // Definindo os meses em português
            string[] months = {
            "janeiro", "fevereiro", "março", "abril", "maio", "junho",
            "julho", "agosto", "setembro", "outubro", "novembro", "dezembro"
        };

            try
            {
                // Separando a data e hora
                string[] dateAndTime = input.Split(new string[] { " às " }, StringSplitOptions.None);
                string datePart = dateAndTime[0];
                string timePart = dateAndTime[1];

                // Separando os componentes da data
                string[] dateComponents = datePart.Split(new string[] { " de " }, StringSplitOptions.None);
                int day = int.Parse(dateComponents[0]);
                string monthString = dateComponents[1];
                int month = Array.IndexOf(months, monthString) + 1;
                int year = int.Parse(dateComponents[2]);

                // Separando os componentes da hora
                string[] timeComponents = timePart.Split(':');
                int hour = int.Parse(timeComponents[0]);
                int minute = int.Parse(timeComponents[1]);

                // Criando o DateTime
                DateTime dateTime = new DateTime(year, month, day, hour, minute, 0);
                string dataFormatted = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                
                // Retornando no formato desejado
                return dataFormatted;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao converter a data: " + ex.Message);
                return null;
            }
        }
    }
}
