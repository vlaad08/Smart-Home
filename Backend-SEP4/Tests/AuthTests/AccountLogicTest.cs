using System.Security.Cryptography;
using System.Text;
using DBComm.Repository;
using DBComm.Shared;
using Moq;
using NuGet.Frameworks;
using WebAPI.Service;

namespace Tests.AuthTests;

public class AccountLogicTest
{
    [Fact]
    public async Task Hashing_works()
    {
        // We know that 1 hashed is 6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b
        byte[] inputBytes = Encoding.UTF8.GetBytes("1");
        string hashedString = "";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }
        Assert.Equal("6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b".ToUpper(),hashedString);
    }
    //As the hashing works, these tests could bear the 
    [Fact]
    public async Task RegisterMember_calls_for_repository()
    {
        
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckExistingUser("TEST")).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
    
        await logic.RegisterMember("TEST", "TEST");
    
        mockRepository.Verify(m => m.CheckExistingUser("TEST"), Times.Once); 
        mockRepository.Verify(m => m.RegisterMember("TEST", It.IsAny<string>()), Times.Once); 
    }
    
    [Fact]
    public async Task Delete_calls_for_repository()
    {
        
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
        
        await logic.Delete("TEST", "TEST");
    
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.DeleteAccount("TEST"), Times.Once); 
    }

    [Fact]
    public async Task RegisterAdmin_call_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckExistingUser("TEST")).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
        
        await logic.RegisterAdmin("TEST", "TEST");
    
        mockRepository.Verify(m => m.CheckExistingUser("TEST"), Times.Once); 
        mockRepository.Verify(m => m.RegisterAdmin("TEST",It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task EditUsername_calls_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
        await logic.EditUsername("TEST", "TEST1", "TEST");
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.EditUsername("TEST","TEST1"), Times.Once);
    }
    
    [Fact]
    public async Task EditPassword_calls_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
        await logic.EditPassword("TEST", "TEST", "TEST1");
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.EditPassword("TEST",It.IsAny<string>(),It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task ToggleAdmin_calls_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckIfAdmin("TEST",It.IsAny<string>(),"TEST1")).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
        await logic.ToggleAdmin("TEST", "TEST", "TEST1");
        mockRepository.Verify(m => m.CheckIfAdmin("TEST",It.IsAny<string>(),"TEST1"), Times.Once); 
        mockRepository.Verify(m => m.ToggleAdmin("TEST1"), Times.Once);
    }

    [Fact]
    public async Task Login_calls_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.Login("TEST", "TEST")).ReturnsAsync(new Member());
        var logic = new AccountLogic(mockRepository.Object);
        
        await logic.Login("TEST", "TEST");
                        // password TEST is hashed to 94EE059335E587E501CC4BF90613E0814F00A7B08BC7C648FD865A2AF6A22CC2
        mockRepository.Verify(m => m.Login("TEST", "94EE059335E587E501CC4BF90613E0814F00A7B08BC7C648FD865A2AF6A22CC2"), Times.Once);
    }

}