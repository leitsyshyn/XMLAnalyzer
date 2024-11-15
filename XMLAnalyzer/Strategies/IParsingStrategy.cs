using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLAnalyzer.Models;
using XMLAnalyzer.Services;

namespace XMLAnalyzer.Strategies
{
    public interface IParsingStrategy
    {
        List<StaffMember> Parse(string xmlFilePath, string? xpathQuery = null, StaffFilter? filter = null);
    }

}
