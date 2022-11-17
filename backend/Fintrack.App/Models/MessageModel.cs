using System.ComponentModel.DataAnnotations;

namespace Fintrack.App.Models;

public class MessageModel
{
    public string Email { get; set; }

    [Required] public string Topic { get; set; }

    [Required] public string Message { get; set; }
}