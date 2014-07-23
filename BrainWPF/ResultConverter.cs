using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BrainWPF
{
    public class ResultConverter : IValueConverter //System.Windows.DependencyObject, 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                switch ((int)value)
                {
                    case 0:
                        return Brushes.White;
                    case -1:
                        return Brushes.Orange;
                    case 1:
                        return Brushes.Red;
                    case 2:
                        return Brushes.Green;
                }
            }
            catch 
            {
                if ((bool)value)
                    return Brushes.Yellow;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
