using BL_Karate.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1.DL_Context;
using _2.RepositoryLayer;

namespace BL_Karate.DomainEntityMapper
{
    public class MapDomainEntity
    {

        //public List<PhotoDetailsDM> MapPhotoDetailsDomainEntity(List<PhotoDetailsDM> photoDetailsDomainModel, List<Photo_Details> photoDetailsEntityModel)
        //{
        //    for (int i = 0; i < photoDetailsEntityModel.Count;i++ )
        //    {
        //        var DomainModel = new PhotoDetailsDM();
        //        DomainModel.Id = photoDetailsEntityModel[i].Id;
        //        DomainModel.UniqueName = photoDetailsEntityModel[i].UniqueName;
        //        DomainModel.ModuleName = photoDetailsEntityModel[i].ModuleName;
        //        DomainModel.FolderName = photoDetailsEntityModel[i].FolderName;
        //        DomainModel.FolderPath = photoDetailsEntityModel[i].FolderPath;
        //        DomainModel.Name = photoDetailsEntityModel[i].Name;
        //        DomainModel.Title = photoDetailsEntityModel[i].Title;
        //        DomainModel.Description = photoDetailsEntityModel[i].Description;
        //        DomainModel.Comments = photoDetailsEntityModel[i].Comments;
        //        DomainModel.OptionOne = photoDetailsEntityModel[i].OptionOne;
        //        photoDetailsDomainModel.Add(DomainModel);
        //    }
        //    return photoDetailsDomainModel;
        //}
    }
}
