using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL_Karate.DomainModels;


namespace BL_Karate
{
    interface IBusinessLayer 
    {
        List<PhotoDetailsDM> GetPhotoDetailsDM();
    }
}
