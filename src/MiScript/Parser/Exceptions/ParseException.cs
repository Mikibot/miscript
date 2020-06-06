using Miki.Localization;
using Miki.Localization.Exceptions;
using MiScript.Parser.Models;

namespace MiScript.Parser.Exceptions
{
    internal class ParseException : LocalizedException
    {
        public override IResource LocaleResource 
            => new LanguageResource("error_miscript_parse", _message);

        protected ParseContext _context;

        private string _message;

        public ParseException(ParseContext context, string message)
        {
            _message = message;
            _context = context;
        }
    }
}
