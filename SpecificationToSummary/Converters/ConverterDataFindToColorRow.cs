using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SpecificationToSummary
{
    /// <summary>
    /// Класс содержащий конвертер данных о позиции в цвет строки в таблице
    /// При отсутствии у позиции всех данных (за исключением самого названия позиции) строка с такой позициией окрашивается в красный цвет
    /// При отсутсвии у позиции одного из показателей ("Масса за единицу", "номер позиции", "тип, марка позиции", "единица измерения", "код позиции", "количество") строка с такой позицией окрашивается в желтый цвет
    /// </summary>
    public class ConverterDataFindToColorRow : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataFind a)
            {
                if (a.Mass == "" && a.Position == "" && a.TypePosition == "" && a.Unit == "" && a.Code == "" && a.Count == "")
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else if (a.Mass == "" || a.Position == "" || a.TypePosition == "" || a.Unit == "" || a.Code == "" || a.Count == "")
                {
                    return new SolidColorBrush(Colors.Yellow);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}