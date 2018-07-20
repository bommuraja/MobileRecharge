using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Karate.DomainModels
{
    public class PhotoDetailsDM
    {
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public string ModuleName { get; set; }
        public string FolderName { get; set; }
        public string FolderPath { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string OptionOne { get; set; }
    }
}
