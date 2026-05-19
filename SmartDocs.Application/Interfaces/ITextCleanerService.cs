using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Application.Interfaces
{
    public interface ITextCleanerService
    {
        string CleanText(string text);
    }
}
