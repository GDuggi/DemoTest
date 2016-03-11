namespace Sempra.Confirm.InBound.ImageEdit
{
    public interface IImageHolder
    {
        int TotalPages { get; }
        bool ImageModified { get; }
        byte[] GetImageBytes();
    }
}