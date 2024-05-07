using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class LightReading
{
    [Key]
    public string Id { get; set; }
    public double Value { get; set; }
    public DateTime ReadAt { get; set; }
    
    public Room Room { get; set; }

    public LightReading()
    {
        
    }
    
    
}