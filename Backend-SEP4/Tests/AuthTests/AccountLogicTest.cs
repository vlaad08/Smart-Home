using System.ComponentModel.DataAnnotations;
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
    [Fact]
    public async Task RegisterMember_calls_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckExistingUser("TEST")).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
    
        await logic.RegisterMember("TEST", "TEST1234");
    
        mockRepository.Verify(m => m.CheckExistingUser("TEST"), Times.Once); 
        mockRepository.Verify(m => m.RegisterMember("TEST", It.IsAny<string>()), Times.Once); 
    }
    
    [Fact]
    public async Task RegisterMember_returns_null_if_member_is_not_added_and_no_exceptions_are_thrown()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckExistingUser("TEST")).ReturnsAsync(false);
        var logic = new AccountLogic(mockRepository.Object);
    
        var returned = await logic.RegisterMember("TEST", "TEST1234");
        
        Assert.Null(returned);
    }
    
    [Fact]
    public async Task RegisterMember_throws_8_char_exception()
    {
        var mockRepository = new Mock<IAccountRepository>();
        var logic = new AccountLogic(mockRepository.Object);
    
        var exception = await Assert.ThrowsAsync<ValidationException>(()=> logic.RegisterMember("TEST", "TEST123"));
    
        mockRepository.Verify(m => m.CheckExistingUser("TEST"), Times.Never); 
        mockRepository.Verify(m => m.RegisterMember("TEST", It.IsAny<string>()), Times.Never);
        Assert.Equal("Password needs to be at least 8 characters.",exception.Message);
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
    public async Task Delete_throws_exception_up()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ThrowsAsync(new Exception("User with username TEST doesn't exist"));
        var logic = new AccountLogic(mockRepository.Object);
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.Delete("TEST", "TEST"));
    
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.DeleteAccount("TEST"), Times.Never);
        Assert.Equal("User with username TEST doesn't exist",exception.Message);
    }
    
    
    [Fact]
    public async Task EditUsername_calls_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        var logic = new AccountLogic(mockRepository.Object);
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST", It.IsAny<string>())).ReturnsAsync(true);
        mockRepository.Setup(m => m.CheckExistingUser("TEST1")).ReturnsAsync(true);
        
        await logic.EditUsername("TEST", "TEST1", "PW");
        
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.CheckExistingUser("TEST1"), Times.Once); 
        mockRepository.Verify(m => m.EditUsername("TEST","TEST1"), Times.Once);
    }

    [Fact]
    public async Task EditUsername_throws_exception_up()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ThrowsAsync(new Exception("User with username TEST doesn't exist"));
        var logic = new AccountLogic(mockRepository.Object);
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.EditUsername("TEST", "TEST","PW"));
        
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.EditUsername("TEST","TEST1"), Times.Never);
        Assert.Equal("User with username TEST doesn't exist",exception.Message);
    }
    
    [Fact]
    public async Task EditPassword_calls_for_repository()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
        await logic.EditPassword("TEST", "TEST", "TEST1234");
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.EditPassword("TEST",It.IsAny<string>(),It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task EditPassword_throws_exception_up()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ThrowsAsync(new Exception("User with username TEST doesn't exist"));
        var logic = new AccountLogic(mockRepository.Object);
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.EditPassword("TEST", "PW1","TEST1234"));
        
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.EditPassword("TEST","TEST1",It.IsAny<string>()), Times.Never);
        Assert.Equal("User with username TEST doesn't exist",exception.Message);
    }
    
    [Fact]
    public async Task EditPassword_throws_exception_up_on_null_password()
    {
        var mockRepository = new Mock<IAccountRepository>();
        var logic = new AccountLogic(mockRepository.Object);
        
        var exception = await Assert.ThrowsAsync<ValidationException>(()=>logic.EditPassword("TEST", "PW",""));
        
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Never); 
        mockRepository.Verify(m => m.EditPassword("TEST","PW",It.IsAny<string>()), Times.Never);
        Assert.Equal("Password needs to be at least 8 characters.",exception.Message);
    }
    
    [Fact]
    public async Task EditPassword_throws_exception_upon_same_password()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckNonExistingUser("TEST",It.IsAny<string>())).ReturnsAsync(true);
        var logic = new AccountLogic(mockRepository.Object);
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.EditPassword("TEST", "PW123456","PW123456"));
        
        mockRepository.Verify(m => m.CheckNonExistingUser("TEST",It.IsAny<string>()), Times.Once); 
        mockRepository.Verify(m => m.EditPassword("TEST","PW",It.IsAny<string>()), Times.Never);
        Assert.Equal("Cannot set same password",exception.Message);
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
    public async Task ToggleAdmin_throws_exception_up()
    {
        var mockRepository = new Mock<IAccountRepository>();
        mockRepository.Setup(m => m.CheckIfAdmin("TEST",It.IsAny<string>(),"TestUsername")).ThrowsAsync(new Exception("User with username TestUsername doesn't exist"));
        var logic = new AccountLogic(mockRepository.Object);
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.ToggleAdmin("TEST", "PW1","TestUsername"));
        
        mockRepository.Verify(m => m.CheckIfAdmin("TEST",It.IsAny<string>(),"TestUsername"), Times.Once); 
        mockRepository.Verify(m => m.ToggleAdmin("TestUsername"), Times.Never);
        Assert.Equal("User with username TestUsername doesn't exist",exception.Message);
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

    [Fact]
    public async Task Login_throws_exception_upon_null_pw()
    {
        var mockRepository = new Mock<IAccountRepository>();
        var logic = new AccountLogic(mockRepository.Object);
    
        var exception = await Assert.ThrowsAsync<ValidationException>(() => logic.Login("username", ""));

        mockRepository.Verify(m => m.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Never); 
        Assert.Equal("Password cannot be null", exception.Message);
    }
    
    [Fact]
    public async Task Login_throws_exception_upon_null_username()
    {
        var mockRepository = new Mock<IAccountRepository>();
        var logic = new AccountLogic(mockRepository.Object);
    
        var exception = await Assert.ThrowsAsync<ValidationException>(() => logic.Login("", "pw"));

        mockRepository.Verify(m => m.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Never); 
        Assert.Equal("Username cannot be null", exception.Message);
    }
    
    [Fact]
    public async Task Login_throws_exception_up()
    {
        var mockRepository = new Mock<IAccountRepository>();
        var logic = new AccountLogic(mockRepository.Object);
        mockRepository.Setup(m => m.Login(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Invalid username or password"));
        
        var exception = await Assert.ThrowsAsync<Exception>(() => logic.Login("username", "pw"));

        mockRepository.Verify(m => m.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once); 
        Assert.Equal("Invalid username or password", exception.Message);
    }


}