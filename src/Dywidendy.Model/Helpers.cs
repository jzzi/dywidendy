using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Dywidendy.Model
{
    public static class Helpers
    {
        public static IEnumerable<IChangeDepositEvent> GetEventsFromFile(string path)
        {
            return File.ReadLines(path).Where(p => p.Trim().Length > 0).Select(p =>
            {
                var parts = p.Split(';');
                return new MoneyChanged(decimal.Parse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                    decimal.Parse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture),
                    DateTime.ParseExact(parts[0], "yyyy-MM-dd", CultureInfo.InvariantCulture));
            });
        }
    }
}