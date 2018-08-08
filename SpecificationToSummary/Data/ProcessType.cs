namespace SpecificationToSummary
{
    /// <summary>
    /// Перечисление с указанием на какой стадии находится процесс работы программы
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// NotReady - программа не готова к выполеннию работы
        /// </summary>
        NotReady,
        /// <summary>
        /// Ready - программа готова к выполеннию работы
        /// </summary>
        Ready,
        /// <summary>
        /// Working - выполнение работы идет
        /// </summary>
        Working,
        /// <summary>
        /// Completed - выполнение работы завершено
        /// </summary>
        Completed,
        /// <summary>
        /// Stopped - выполнение работы остановлено пользователем
        /// </summary>
        Stopped,
        /// <summary>
        /// Error - выполнение работы завершилось с ошибкой
        /// </summary>
        Error
    }
}