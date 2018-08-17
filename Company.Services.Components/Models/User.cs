namespace Company.Services.Components.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [StringLength(10)]
        public string Password { get; set; }
    }
}
