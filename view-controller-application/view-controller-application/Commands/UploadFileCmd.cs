using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace view_controller_application.Commands
{
    public class UploadFileCmd
    {
        public IFormFile File { get; set; }
    }
}
