using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dywidendy.Model
{
    public static class Helpers
    {
        public static IEnumerable<IChangeDepositEvent> GetEventsFromFile(string path)
        {
            return File.ReadLines(path).Select(p =>
            {
                var parts = p.Split(';');
                return new ChangeDepositEvent(decimal.Parse(parts[0]), decimal.Parse(parts[1]), DateTime.Parse(parts[2]));
            });
        }
    }
}