using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.DoorTests;

public class DoorLogicTest
{
    private DoorLogic _logic;
    private Mock<IDoorRepository> _mockRepository;
    
    [Fact]
    public async Task ChangeLockPassword_ValidInput()
    {
        _mockRepository = new Mock<IDoorRepository>();
        _logic = new DoorLogic(_mockRepository.Object);
        string homeId = "testHomeId";
        int newPassword = 1234;

        _mockRepository.Setup(r => r.CheckIfDoorExist(homeId)).ReturnsAsync(true);
        await _logic.ChangeLockPassword(homeId, newPassword);
        _mockRepository.Verify(r => r.ChangePassword(homeId, newPassword), Times.Once);
    }

    [Fact]
    public async Task ChangeLockPassword_EmptyHomeId_ThrowsException()
    {
        _mockRepository = new Mock<IDoorRepository>();
        _logic = new DoorLogic(_mockRepository.Object);
        string emptyHomeId = "";
        int newPassword = 1234;
        await Assert.ThrowsAsync<Exception>(() => _logic.ChangeLockPassword(emptyHomeId, newPassword));
    }

    [Fact]
    public async Task ChangeLockPassword_DoorDoesNotExist()
    {
        
        _mockRepository = new Mock<IDoorRepository>();
        _logic = new DoorLogic(_mockRepository.Object);
        string homeId = "nonExistentHomeId";
        int newPassword = 1234;
        _mockRepository.Setup(r => r.CheckIfDoorExist(homeId)).ThrowsAsync(new Exception("Door does not exist."));
        await Assert.ThrowsAsync<Exception>(() => _logic.ChangeLockPassword(homeId, newPassword));
    }

}