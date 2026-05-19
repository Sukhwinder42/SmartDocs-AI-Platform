using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Application.DTOs
{
    public class AskQuestionDto
    {
        public Guid DocumentId { get; set; }

        public string Question { get; set; }
    }
}
