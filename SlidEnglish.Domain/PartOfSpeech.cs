namespace SlidEnglish.Domain
{
    /// <summary>
    /// Часть речи
    /// </summary>
    public enum PartOfSpeech
    {
        None = 0,

        /// <summary>
        /// Глагол
        /// </summary>
        Verb = 1,

        /// <summary>
        /// Существительное
        /// </summary>
        Noun = 2,

        /// <summary>
        /// Прилагательное
        /// </summary>
        Adjective = 3,

        /// <summary>
        /// Предлог
        /// </summary>
        Preposition = 4,

        /// <summary>
        /// Местоимение
        /// </summary>
        Pronoun = 5,

        /// <summary>
        /// Артикль
        /// </summary>
        Article = 6,
    }
}
