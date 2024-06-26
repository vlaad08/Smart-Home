﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DBComm.Shared;

public class Home
{
    [Key]
    public string Id { get; set; }
    [JsonIgnore]
    public ICollection<Notification>? Notifications { get; set; }
    [JsonIgnore]
    public ICollection<Room>? Rooms { get; set; }
    [JsonIgnore]
    public ICollection<Door> Doors { get; set; }
    [JsonIgnore]
    public ICollection<Member> Members { get; set; }

    public Home(string id)
    {
        Id = id;
    }
    
}