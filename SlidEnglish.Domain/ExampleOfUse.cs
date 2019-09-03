using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.Domain
{
    public class ExampleOfUse
    {
        public int LexicalUnitId { get; set; }
        public virtual LexicalUnit LexicalUnit { get; set; }

        [Required]
        public string Example { get; set; }

        public ExampleOfUseAttribute Attribute { get; set; }
    }
}
