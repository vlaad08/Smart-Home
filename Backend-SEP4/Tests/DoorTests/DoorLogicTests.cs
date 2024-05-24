// using System.Security.Cryptography;
// using System.Text;
// using DBComm.Logic;
// using DBComm.Repository;
// using Moq;

// namespace Tests.DoorTests;

// public class DoorLogicTest
// {
//     private DoorLogic _logic;
//     private Mock<IDoorRepository> _mockRepository;
//     private Mock<INotificationRepository> _notificationRepository;
//     // [Fact]
//     // public async Task ChangeLockPassword_ValidInput()
//     // {
//     //     _mockRepository = new Mock<IDoorRepository>();
//     //     _logic = new DoorLogic(_mockRepository.Object, _notificationRepository.Object);
//     //     string homeId = "testHomeId";
//     //     string newPassword = "1234";
//     //     _mockRepository.Setup(r => r.CheckIfDoorExist(homeId)).ReturnsAsync(true);
//     //     await _logic.ChangeLockPassword(homeId, newPassword);
//     //     byte[] inputBytes = Encoding.UTF8.GetBytes(newPassword);
//     //     string hashedString;
//     //     using (SHA256 sha256 = SHA256.Create())
//     //     {
//     //         byte[] hashBytes = sha256.ComputeHash(inputBytes);
//     //         hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
//     //     }
//     //     _mockRepository.Verify(r => r.ChangePassword(homeId, hashedString), Times.Once);
//     // }
//     // [Fact]
//     // public async Task ChangeLockPassword_EmptyHomeId_ThrowsException()
//     // {
//     //     _mockRepository = new Mock<IDoorRepository>();
//     //     _logic = new DoorLogic(_mockRepository.Object, _notificationRepository.Object);
//     //     string emptyHomeId = "";
//     //     string newPassword = "1234";
//     //     await Assert.ThrowsAsync<Exception>(() => _logic.ChangeLockPassword(emptyHomeId, newPassword));
//     // }
//     // [Fact]
//     // public async Task ChangeLockPassword_DoorDoesNotExist()
//     // {
//     //     _mockRepository = new Mock<IDoorRepository>();
//     //     _logic = new DoorLogic(_mockRepository.Object, _notificationRepository.Object);
//     //     string homeId = "nonExistentHomeId";
//     //     string newPassword = "1234";
//     //     _mockRepository.Setup(r => r.CheckIfDoorExist(homeId)).ThrowsAsync(new Exception("Door does not exist."));
//     //     await Assert.ThrowsAsync<Exception>(() => _logic.ChangeLockPassword(homeId, newPassword));
//     // }
// }