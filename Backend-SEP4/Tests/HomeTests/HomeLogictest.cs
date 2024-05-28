using System.ComponentModel.DataAnnotations;
using Moq;

namespace Tests.HomeTests;

public class HomeLogictest
{
    [Fact]
    public async Task GetMemberByHomeId_calls_for_repo()
    {
        var mock = new Mock<IHomeRepository>();
        var logic = new HomeLogic(mock.Object);

        await logic.GetMembersByHomeId("1");
        
        mock.Verify(m=>m.GetMembersByHomeId("1"),Times.Once);
    }

    [Fact]
    public async Task AddMemberToHome_calls_for_repo()
    {
        var mock = new Mock<IHomeRepository>();
        var logic = new HomeLogic(mock.Object);
        mock.Setup(m => m.CheckUserExists("username")).ReturnsAsync(true);

        await logic.AddMemberToHome("username", "1");
        
        mock.Verify(m=>m.AddMemberToHome("username","1"),Times.Once);
    }

    [Fact]
    public async Task AddMemberToHome_throws_exception_upon_null_username()
    {
        var mock = new Mock<IHomeRepository>();
        var logic = new HomeLogic(mock.Object);

        var exception = await Assert.ThrowsAsync<ValidationException>(()=>logic.AddMemberToHome("", "1"));
        
        mock.Verify(m=>m.AddMemberToHome("username","1"),Times.Never);
        Assert.Equal("Username null",exception.Message);
    }
    
    [Fact]
    public async Task AddMemberToHome_throws_exception_upon_user_not_found()
    {
        var mock = new Mock<IHomeRepository>();
        var logic = new HomeLogic(mock.Object);
        mock.Setup(m => m.CheckUserExists("username")).ThrowsAsync(new Exception("No user with that username"));
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.AddMemberToHome("username", "1"));
        
        mock.Verify(m=>m.AddMemberToHome("username","1"),Times.Never);
        Assert.Equal("No user with that username",exception.Message);
    }

    [Fact]
    public async Task RemoveMemberFromHome_calls_for_repo()
    {
        var mock = new Mock<IHomeRepository>();
        var logic = new HomeLogic(mock.Object);
        mock.Setup(m => m.CheckUserExists("username")).ReturnsAsync(true);

        await logic.RemoveMemberFromHome("username");
        
        mock.Verify(m=>m.RemoveMemberFromHome("username"),Times.Once);
    }

    [Fact]
    public async Task RemoveMemberFromHome_throws_exception_upon_null_username()
    {
        var mock = new Mock<IHomeRepository>();
        var logic = new HomeLogic(mock.Object);

        var exception = await Assert.ThrowsAsync<ValidationException>(()=>logic.RemoveMemberFromHome(""));
        
        mock.Verify(m=>m.RemoveMemberFromHome(""),Times.Never);
        Assert.Equal("Username cannot be null",exception.Message);
    }
    
    [Fact]
    public async Task RemoveMemberFromHome_throws_exception_upon_user_not_found()
    {
        var mock = new Mock<IHomeRepository>();
        var logic = new HomeLogic(mock.Object);
        mock.Setup(m => m.CheckUserExists("username")).ThrowsAsync(new Exception("No user with that username"));
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.RemoveMemberFromHome("username"));
        
        mock.Verify(m=>m.RemoveMemberFromHome("username"),Times.Never);
        Assert.Equal("No user with that username",exception.Message);
    }
}