using GalaSoft.MvvmLight;

namespace SpecificationToSummary
{
    /// <summary>
    /// Класс для хранения данных о прогрессе работы программы
    /// </summary>
    public class ProgressReport : ViewModelBase
    {
        private string _processedFile;
        /// <summary>
        /// ProcessedFile - файл (путь к файлу) обрабатываемый в данный момент
        /// </summary>
        public string ProcessedFile
        {
            get => _processedFile;
            set => Set("ProcessedFile", ref _processedFile, value);
        }

        private string _processedMessage;
        /// <summary>
        /// ProcessedMessage - сообщение для вывода на всплывающем окне о результатах работы
        /// при изменении сообщения, открывается всплывающее окно
        /// </summary>
        public string ProcessedMessage
        {
            get => _processedMessage;
            set
            {
                Set("ProcessedMessage", ref _processedMessage, value);
                if (!string.IsNullOrEmpty(value))
                {
                    IsOpenMessage = true;
                }
            }
        }

        private int _progressValue = 0;
        /// <summary>
        /// ProgressValue - значение обработанных файлов
        /// </summary>
        public int ProgressValue
        {
            get => _progressValue;
            set => Set("ProgressValue", ref _progressValue, value);
        }

        private int _progressMax =0;
        /// <summary>
        /// ProgressMax - общее количество обрабатываемых файлов
        /// </summary>
        public int ProgressMax
        {
            get => _progressMax;
            set => Set("ProgressMax", ref _progressMax, value);
        }

        private ProcessType _processType = ProcessType.NotReady;
        /// <summary>
        /// ProcessType - стадия выполнения работы
        /// </summary>
        public ProcessType ProcessType
        {
            get => _processType;
            set => Set("ProcessType", ref _processType, value);
        }

        private bool _isOpenMessage = false;
        /// <summary>
        /// IsOpenMessage - открыто ли всплывающее окно с сообщением о результатах работы
        /// </summary>
        public bool IsOpenMessage
        {
            get => _isOpenMessage;
            set => Set("IsOpenMessage", ref _isOpenMessage, value);
        }
    }
}