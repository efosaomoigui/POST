using POST.Core.Enums;
using POST.CORE.DTO;

namespace POST.Core.DTO
{
    public class ImageDTO : BaseDomainDTO
    {
        public int ImageId { get; set; }
        public ImageFileType FileType { get; set; }
        public string ImageString { get; set; }
        public string PartnerFullName { get; set; }
       
    }
}
