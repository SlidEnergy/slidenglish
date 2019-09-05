using System;
using System.Collections.Generic;
using System.Text;

namespace SlidEnglish.Domain
{
    public enum RelationAttribute
    {
        /// <summary>
        /// Нет аттрибута
        /// </summary>
        None = 0,

        /// <summary>
        /// Слово синоним
        /// </summary>
        Synonym = 1,

        /// <summary>
        /// Однокоренное
        /// </summary>
        RootWord = 2,

        /// <summary>
        /// Одна из форм глагола
        /// </summary>
        VerbForm = 3
    }
}
