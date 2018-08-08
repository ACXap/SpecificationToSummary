using System.Collections.ObjectModel;

namespace SpecificationToSummary
{
    /// <summary>
    /// Класс с данными для передачи в метод чтения файлов спецификаций
    /// CollectionFiles - коллеция файлов спецификаций
    /// CollectionResults - коллекция позиций найденых в файлах спецификациях
    /// </summary>
    public class DataRead
    {
        /// <summary>
        /// CollectionFiles - коллеция файлов спецификаций
        /// </summary>
        public ObservableCollection<FileExcel> CollectionFiles { get; set; }
        /// <summary>
        /// CollectionResults - коллекция позиций найденых в файлах спецификациях
        /// </summary>
        public ObservableCollection<DataFind> CollectionResults { get; set; }
    }
}