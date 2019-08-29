using System;
using System.Collections.Generic;
using System.Text;

namespace SlidEnglish.Domain
{
    /// <summary>
    /// Атрибут примера использования слова или фразы
    /// </summary>
    public enum ExampleOfUseAttribute
    {
        None = 0,

        /// <summary>
        /// Пример взятый из какого-либо источника
        /// </summary>
        ExternalExample = 1,

        /// <summary>
        /// Пример пользователя
        /// </summary>
        UserExample = 2
    }
}
