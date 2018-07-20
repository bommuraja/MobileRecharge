using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL_Karate.DomainModels;
using BL_Karate.DomainEntityMapper;
//using _1.DL_Context;
using _2.RepositoryLayer;

namespace BL_Karate
{
    public class BusinessLayer : IBusinessLayer
    {
        private KarateRepository RepositoryLayer;
        private MapDomainEntity Map;

        public BusinessLayer(KarateRepository repositoryLayer, MapDomainEntity map)
        {
            RepositoryLayer = repositoryLayer;
            Map = map;
            
        }

        public List<PhotoDetailsDM> GetPhotoDetailsDM()
        {
            //var DM = RepositoryLayer.GetPhotoDetails();
            var BM = new List<PhotoDetailsDM>();
            //return Map.MapPhotoDetailsDomainEntity(BM,DM);
            return BM;
        }
    }
}
