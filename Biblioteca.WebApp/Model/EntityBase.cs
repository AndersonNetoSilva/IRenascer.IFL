using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
