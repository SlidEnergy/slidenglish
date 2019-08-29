using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlidEnglish.Domain
{
    public class ExampleOfUse: IUniqueObject
    {
        public int Id { get; set; }

        [Required]
        public string Example { get; set; }

        public ExampleOfUseAttribute Attribute { get; set; }
    }
}
