using GalaSoft.MvvmLight;

namespace SpecificationToSummary
{
    /// <summary>
    /// Класс для хранения позиции найденой в файле спецификации
    /// </summary>
    public class DataFind : ViewModelBase
    {
        private bool _isCheck = false;
        /// <summary>
        /// IsCheck - выделена ли позиция, для редактирования, для сохранения в итоговый файл
        /// </summary>
        public bool IsCheck
        {
            get => _isCheck;
            set => Set("IsCheck", ref _isCheck, value);
        }

        private string _position;
        /// <summary>
        /// Position - номер позиции в файле спецификации 
        /// </summary>
        public string Position
        {
            get => _position;
            set => Set("Position", ref _position, value);
        }

        private string _nameFile;
        /// <summary>
        /// NameFile - имя файла спецификации в котором содержится позиция
        /// </summary>
        public string NameFile
        {
            get => _nameFile;
            set => Set("NameFile", ref _nameFile, value);
        }
        private string _namePosition;
        /// <summary>
        /// NamePosition - наименование и техническая характеристика позиции
        /// </summary>
        public string NamePosition
        {
            get => _namePosition;
            set => Set("NamePosition", ref _namePosition, value);
        }
        private string _typePosition;
        /// <summary>
        /// TypePosition - тип, марка позиции, обозначение документа, опросного листа
        /// </summary>
        public string TypePosition
        {
            get => _typePosition;
            set => Set("TypePosition", ref _typePosition, value);
        }

        private string _code;
        /// <summary>
        /// Code - код оборудования, изделия, материала
        /// </summary>
        public string Code
        {
            get => _code;
            set => Set("Code", ref _code, value);
        }

        private string _factory;
        /// <summary>
        /// Factory - завод изготовитель
        /// </summary>
        public string Factory
        {
            get => _factory;
            set => Set("Factory", ref _factory, value);
        }
        private string _unit;
        /// <summary>
        /// Unit - единица измерения (шт., комплекты)
        /// </summary>
        public string Unit
        {
            get => _unit;
            set => Set("Unit", ref _unit, value);
        }
        private string _count;
        /// <summary>
        /// Count - количество 
        /// </summary>
        public string Count
        {
            get => _count;
            set => Set("Count", ref _count, value);
        }
        private string _mass;
        /// <summary>
        /// Mass - масса единицы, в кг.
        /// </summary>
        public string Mass
        {
            get => _mass;
            set => Set("Mass", ref _mass, value);
        }
    }
}