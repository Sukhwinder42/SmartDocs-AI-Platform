using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Application.Interfaces
{
    public interface ITextChunkService
    {
        List<string> SplitIntoChunks(string text, int chunkSize = 500);
    }
}
