using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrack.Database.Entities;

public class Log
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Message { get; set; }

    [MaxLength(128)] public string Level { get; set; }

    [DataType(DataType.DateTime)]
    [Column(TypeName = "datetime")]
    public DateTime TimeStamp { get; set; }

    public string Exception { get; set; }
}