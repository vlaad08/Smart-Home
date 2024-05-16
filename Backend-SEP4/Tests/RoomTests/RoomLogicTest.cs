using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.RoomTests;

public class RoomLogicTest
{
    [Fact]
    public async Task AddRoom_calls_for_repository()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        mock.Setup(m => m.CheckExistingRoom("test", "test")).ReturnsAsync(true);
        await logic.AddRoom("test", "test","test");
        
        mock.Verify(m=>m.CheckExistingRoom("test","test"));
        mock.Verify(m=>m.AddRoom("test","test","test"));
    }
    
    [Fact]
    public async Task AddRoom_throws_custom_error()
    {
        var mock = new Mock<IRoomRepository>();
        mock.Setup(m => m.CheckExistingRoom("test", "test"))
            .ThrowsAsync(new Exception("Room test already exists in home test"));

        var logic = new RoomLogic(mock.Object);
        var exception = await Assert.ThrowsAsync<Exception>(() => logic.AddRoom("test", "test", "test"));

        Assert.Equal("Room test already exists in home test", exception.Message);
        mock.Verify(m => m.CheckExistingRoom("test", "test"), Times.Once);
        mock.Verify(m => m.AddRoom(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteRoom_calls_for_repository()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        mock.Setup(m => m.CheckNonExistingRoom("test")).ReturnsAsync(true);
        
        await logic.DeleteRoom("test");
        
        mock.Verify(m=>m.CheckNonExistingRoom("test"));
        mock.Verify(m=>m.DeleteRoom("test"));
    }
    
    [Fact]
    public async Task DeleteRoom_throws_custom_error()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        mock.Setup(m => m.CheckNonExistingRoom("test")).ThrowsAsync(new Exception("Room test doesn't exist in home"));
        var exception = await Assert.ThrowsAsync<Exception>(() => logic.DeleteRoom("test"));
        
        Assert.Equal("Room test doesn't exist in home",exception.Message);
        mock.Verify(m=>m.CheckNonExistingRoom("test"));
        mock.Verify(m=>m.DeleteRoom(It.IsAny<string>()),Times.Never);
    }
    
    [Fact]
    public async Task EditRoom_calls_for_repository()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        mock.Setup(m => m.CheckNonExistingRoom("test")).ReturnsAsync(true);
        
        await logic.EditRoom("test");
        
        mock.Verify(m=>m.CheckNonExistingRoom("test"));
        mock.Verify(m=>m.EditRoom("test",null,null));
    }
    
    [Fact]
    public async Task EditRoom_throws_custom_error()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        mock.Setup(m => m.CheckNonExistingRoom("test")).ThrowsAsync(new Exception("Room test doesn't exist in home"));
        var exception = await Assert.ThrowsAsync<Exception>(() => logic.EditRoom("test"));
        
        Assert.Equal("Room test doesn't exist in home",exception.Message);
        mock.Verify(m=>m.CheckNonExistingRoom("test"));
        mock.Verify(m=>m.EditRoom(It.IsAny<string>(),null,null),Times.Never);
    }
    
    [Fact]
    public async Task GetAllRooms_calls_for_repository()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        
        await logic.GetAllRooms("Test","1");
        
        mock.Verify(m=>m.GetAllRooms("Test","1"));
    }

    [Fact]
    public async Task GetAllRooms_throws_custom_error()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        mock.Setup(m => m.GetAllRooms("test","2")).ThrowsAsync(new Exception("No room with device 2 or given wrong house ID"));
        var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetAllRooms("test","2"));
        
        Assert.Equal("No room with device 2 or given wrong house ID",exception.Message);
        mock.Verify(m=>m.GetAllRooms("test","2"));
    }
    
    [Fact]
    public async Task GetRoomData_calls_for_repository()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        
        await logic.GetRoomData("Test","1");
        
        mock.Verify(m=>m.GetRoomData("Test","1",false,false,false));
    }
    [Fact]
    public async Task GetRoomData_throws_custom_error()
    {
        var mock = new Mock<IRoomRepository>();
        var logic = new RoomLogic(mock.Object);
        mock.Setup(m => m.GetRoomData("test","2",It.IsAny<bool>(),It.IsAny<bool>(),It.IsAny<bool>())).ThrowsAsync(new Exception("No room with device 2"));
        var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetRoomData("test","2",true));
        
        Assert.Equal("No room with device 2",exception.Message);
        mock.Verify(m=>m.GetRoomData("test","2",true,false,false));
    }
   
}