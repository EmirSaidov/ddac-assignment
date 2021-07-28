using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Helper
{
    public static class IFormFileExtension
    {
        public static string getExtension(IFormFile file) {
            return Path.GetExtension(file.FileName);
        }
    }
}
