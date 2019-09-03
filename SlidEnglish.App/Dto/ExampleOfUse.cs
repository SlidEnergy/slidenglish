using SlidEnglish.Domain;
using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.App.Dto
{
    public class ExampleOfUse
    {
        [Required]
        public string Example { get; set; }

        public ExampleOfUseAttribute Attribute { get; set; }
    }
}
