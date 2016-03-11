using System;

namespace DBAccess
{
    public interface IImagesDal
    {
        ImagesDto GetByDocId(Int32 inboundId, ImagesDtoType imagesDtoType);
        Int32 Insert(string originalFileName, string markupFileName, ImagesDto pData);
        Int32 Insert(ImagesDto pData);
        void Update(ImagesDto pData);
        void Delete(ImagesDto pData);
        ImagesDto SwitchImagesDtoType(ImagesDto pData, Int32 newDocId);
    }
}
