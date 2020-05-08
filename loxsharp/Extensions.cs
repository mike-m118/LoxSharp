using System;
using System.Collections.Generic;
using System.Text;

namespace loxsharp
{
    static class Extensions
    {
        public static bool IsAlpha(this char c)
        {
            return char.IsLetter(c) || c == '_';
        }
    }
}
