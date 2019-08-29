using System;
using System.Collections.Generic;
using System.Text;

namespace SlidEnglish.Domain
{
    public enum LexicalUnitInputAttribute
    {
        None = 0,

        /// <summary>
        /// Добавлены пользователем
        /// </summary>
        UserInput = 1,

        /// <summary>
        /// Добавлены при переводе
        /// </summary>
        TranslateInput = 2
    }
}
