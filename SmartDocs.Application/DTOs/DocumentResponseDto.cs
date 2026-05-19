using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Application.DTOs
{
    public class DocumentResponseDto
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string OriginalFileName { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
