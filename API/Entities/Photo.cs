using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")] //this allows to create db table named "Photos" instead of the entity name "Photo", using System.ComponentModel.DataAnnotations.Schema; GREAT!! 
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }        
        public AppUser AppUser { get; set; }    
        public int AppUserId { get; set; }
    }
}