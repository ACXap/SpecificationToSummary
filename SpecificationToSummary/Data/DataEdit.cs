using System.Collections.ObjectModel;

namespace SpecificationToSummary
{
    /// <summary>
    /// Класс с данными для передачи в метод редактирования итогового файла
    /// CollectionResults - коллекция позиций найденых в файлах спецификациях
    /// </summary>
    public class DataEdit
    {
        /// <summary>
        /// CollectionResults - коллекция позиций найденых в файлах спецификациях
        /// </summary>
        public ObservableCollection<DataFind> CollectionResults { get; set; }
    }
}