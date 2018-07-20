using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using _1.DL_Context;
using System.Data.Entity;

namespace _2.RepositoryLayer
{
    public class KarateRepository 
    {
        private KarateDBContext Context;

        public KarateRepository(KarateDBContext context)
        {
            Context = context;
        }

        ////TODO
        //public DbSet<Photo_Details> GetPhotoDetails(int i)
        //{ 
        
        //}
        //public List<Photo_Details> GetPhotoDetails()
        //{
        //    try
        //    {
        //        return Context.Photo_Details.ToList();

        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw ex;
        //    }
        //}

      

       
    }
}
