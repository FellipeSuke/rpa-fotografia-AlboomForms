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
        public static DateTime ConvertToDateTime(string dateString)
        {
            // Definir os nomes dos meses em português
            var culture = new CultureInfo("pt-BR");
            var dateFormat = "dd 'de' MMMM 'de' yyyy 'às' HH:mm";

            DateTime dateTime = DateTime.ParseExact(dateString, dateFormat, culture);
            return dateTime;
        }
    }

}
