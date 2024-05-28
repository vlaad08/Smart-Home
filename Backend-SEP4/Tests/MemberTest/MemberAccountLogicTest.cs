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
        string existingUsername = "testuser";
        _mockRepository = new Mock<IAccountRepository>();
        _logic = new AccountLogic(_mockRepository.Object);
        _mockRepository.Setup(r => r.CheckExistingUser(existingUsername)).ThrowsAsync(new Exception("Simulated exception"));
        await Assert.ThrowsAsync<Exception>(() => _logic.RegisterMember(existingUsername, "password123"));
    }

    [Fact]
    public async Task RegisterMember_Exception_When_Empty_Data()
    {
        _mockRepository = new Mock<IAccountRepository>();
        _logic = new AccountLogic(_mockRepository.Object);
        
        await Assert.ThrowsAsync<ValidationException>(() => _logic.RegisterMember("", "Password"));
        await Assert.ThrowsAsync<ValidationException>(() => _logic.RegisterMember("TestUser", ""));
        await Assert.ThrowsAsync<ValidationException>(() => _logic.RegisterMember("", ""));
    }

   /* [Fact]
    public async Task RemoveMemberFromHouse_ValidInput()
    {
        string username = "testuser"; 
        _mockRepository = new Mock<IAccountRepository>();
        _logic = new AccountLogic(_mockRepository.Object);
        _mockRepository.Setup(r => r.CheckExistingUser(username)).ReturnsAsync(true);
        await _logic.RemoveMemberFromHouse(username);
    }*/

    // [Fact]
    // public async Task RemoveMemberFromHouse_UserDoesNotExist_ThrowsException()
    // {
    //     string username = "nonexistentuser";
    //     _mockRepository = new Mock<IAccountRepository>();
    //     _logic = new AccountLogic(_mockRepository.Object);
    //     _mockRepository.Setup(r => r.CheckExistingUser(username)).ReturnsAsync(false);
    //     var exception = await Assert.ThrowsAsync<Exception>(() => _logic.RemoveMemberFromHouse(username));
    //     Assert.Equal("User does not exist.", exception.Message);
    // }

//     [Fact]
//     public async Task RemoveMemberFromHouse_RepositoryException()
//     {
//         string username = "testuser";
//         _mockRepository = new Mock<IAccountRepository>();
//         _logic = new AccountLogic(_mockRepository.Object);
//         _mockRepository.Setup(r => r.CheckExistingUser(username)).ThrowsAsync(new Exception("Simulated exception"));
//         await Assert.ThrowsAsync<Exception>(() => _logic.RemoveMemberFromHouse(username));
//     }
// }
}