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