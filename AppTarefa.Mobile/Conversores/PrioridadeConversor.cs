using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTarefa.Mobile.Conversores
{
    class PrioridadeConversor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var prioridade = value as string;
            if (prioridade == "Baixa")
            {
                return App.Current.Resources["PropriedadeBaixa"];
            }
            if (prioridade == "Normal")
            {
                return App.Current.Resources["PropriedadeNormal"];
            }
            if (prioridade == "Alta")
            {
                return App.Current.Resources["PropriedadeAlta"];
            }

            return App.Current.Resources["PropriedadeNormal"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
