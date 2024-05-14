using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class Door
{
    [Key]
    public string Id { get; set; }
    public int LockPassword { get; set; }
    public Home Home { get; set; }

    public Door()
    {
        
    }
    
}