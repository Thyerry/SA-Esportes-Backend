using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class MessageDto
{
    public string Content { get; set; }
    public bool? isValid { get; set; }
    public string[]? ErrorList { get; set; }
}