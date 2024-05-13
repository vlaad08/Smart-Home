using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using DBComm.Logic;
using DBComm.Repository;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebAPI.Service;

namespace Tests.MemberAccountTests;

public class MemberAccountLogicTest
{
    
    
    private AccountLogic _logic;
    private Mock<IAccountRepository> _mockRepository;
    

    [Fact]
    public async Task RegisterMember_ValidInput()
    {
        _mockRepository = new Mock<IAccountRepository>();
        _logic = new AccountLogic(_mockRepository.Object);
        string username = "testuser";
        string password = "testpassword";
        Member member = new Member(username, password);
        _mockRepository.Setup(r => r.RegisterMember(username, password)).ReturnsAsync(member);
        var result = await _logic.RegisterMember(username, password);
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
    }

    
    [Fact]
    public async Task RegisterMember_MemberAlreadyExists()
    {
        string exisitngUsername = "testUsername";
        using (var context = new Context())
        {
            var existingMember = new Member(exisitngUsername, "Password");
            context.member.Add(existingMember);
            await context.SaveChangesAsync();
        }
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
        await Assert.ThrowsAsync<ValidationException>(() => logic.RegisterMember("JanKowalski", ""));
    }

}