using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SlidEnglish.Domain
{
    public class LexicalUnitDescriptor
    {
        public string Text { get; private set; }
        public PartOfSpeech PartOfSpeech { get; private set; }

        private static Dictionary<string, LexicalUnitDescriptor> list;

        public static ReadOnlyCollection<LexicalUnitDescriptor> All {get; private set; }

        private LexicalUnitDescriptor(string text, PartOfSpeech partOfSpeech)
        {
            Text = text;
            PartOfSpeech = partOfSpeech;
        }

        static LexicalUnitDescriptor()
        {
            list = new[]
            {
                new LexicalUnitDescriptor("a", PartOfSpeech.Article),
                new LexicalUnitDescriptor("an", PartOfSpeech.Article),
                new LexicalUnitDescriptor("to", PartOfSpeech.Particle)
            }.ToDictionary(x => x.Text);

            All = new ReadOnlyCollection<LexicalUnitDescriptor>(list.Values.ToList());
        }
    }
}
