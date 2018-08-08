using System.Collections;
using System.Collections.ObjectModel;

namespace SpecificationToSummary
{
    /// <summary>
    /// Класс с данными для передачи в метод объединения позиций в таблице найденых позиций в файлах спецификаций
    /// CollectionResults - коллекция позиций найденых в файлах спецификациях
    /// SelectedItems - коллекция выделеных позиций 
    /// </summary>
    public class DataMerger
    {
        /// <summary>
        /// CollectionResults - коллекция позиций найденых в файлах спецификациях
        /// </summary>
        public ObservableCollection<DataFind> CollectionResults { get; set; }
        /// <summary>
        /// SelectedItems - коллекция выделеных позиций 
        /// </summary>
        public IList SelectedItems { get; set; }
    }
}