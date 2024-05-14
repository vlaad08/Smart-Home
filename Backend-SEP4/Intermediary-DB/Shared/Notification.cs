using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class Notification
{
    [Key]
    public string Id { get; set; }
    public DateTime SendAt { get; set; }
    public string Message { get; set; }
    public Home Home { get; set; }

    public Notification()
    {
        
    }
}