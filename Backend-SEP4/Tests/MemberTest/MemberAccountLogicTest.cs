using System.ComponentModel.DataAnnotations;
using DBComm.Logic;
using DBComm.Repository;
using DBComm.Shared;
using Moq;
using WebAPI.Service;

namespace Tests.MemberTest;
public class MemberAccountLogicTest
{
    private AccountLogic _logic;
    private Mock<IAccountRepository> _mockRepository;
    [Fact]
    public async Task RegisterMember_MemberAlreadyExists_ThrowsException()
    {
        //hardcoded in the database
        string exisitngUsername = "testuser";
        using (var context = new Context())
        {
            var service = new AccountRepository(context);
            var logic = new AccountLogic(service);
            await Assert.ThrowsAsync<Exception>(() => logic.RegisterMember(exisitngUsername, "password123"));
        }
    }
    [Fact]
    public async Task RegisterMember_Exception_When_Empty_Data()
    {
        var mockRepository = new Mock<IAccountRepository>();
        var logic = new AccountLogic(mockRepository.Object);
        await Assert.ThrowsAsync<ValidationException>(() => logic.RegisterMember("", "Password"));
        await Assert.ThrowsAsync<ValidationException>(() => logic.RegisterMember("TestUser", ""));
        await Assert.ThrowsAsync<ValidationException>(() => logic.RegisterMember("", ""));
    }
    [Fact]
    public async Task RemoveMemberFromHouse_ValidInput_ThrowsException()
    {
        string username = "testuser"; 
        _mockRepository = new Mock<IAccountRepository>();
        _mockRepository.Setup(r => r.CheckExistingUser(username)).ReturnsAsync(true);
        using (var context = new Context())
        {
            var service = new AccountRepository(context);
            var logic = new AccountLogic(service);
            await logic.RemoveMemberFromHouse(username);

        }
        
    }

    [Fact]
    public async Task RemoveMemberFromHouse_UserDoesNotExist_ThrowsException()
    {
        _mockRepository = new Mock<IAccountRepository>();
        _logic = new AccountLogic(_mockRepository.Object);
        string username = "nonexistentuser";
        _mockRepository.Setup(r => r.CheckExistingUser(username)).ReturnsAsync(false);
        var exception = await Assert.ThrowsAsync<Exception>(() => _logic.RemoveMemberFromHouse(username));
        Assert.Equal("User does not exist.", exception.Message);
    }
    [Fact]
    public async Task RemoveMemberFromHouse_RepositoryException()
    {
        _mockRepository = new Mock<IAccountRepository>();
        _logic = new AccountLogic(_mockRepository.Object);
        string username = "testuser";
        _mockRepository.Setup(r => r.CheckExistingUser(username)).ThrowsAsync(new Exception("Simulated exception"));
        await Assert.ThrowsAsync<Exception>(() => _logic.RemoveMemberFromHouse(username));
    }
}