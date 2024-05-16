using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class Door
{
    [Key]
    public string Id { get; set; }
    public string LockPassword { get; set; }
    public Home Home { get; set; }
    public bool IsOpen { get; set; }

    public Door()
    {
        
    }
    
}