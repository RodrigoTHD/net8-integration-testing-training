using FakeItEasy;
using MyApp.MockingDependencies.Model;
using MyApp.MockingDependencies.Services;

namespace MyApp.MockingDependencies.IntegrationTests
{
    public class MockingSimpleDependencyTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Mock an IEmailService dependency that sends emails.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldNotifyAsyncReturnTrueWhenEmailIsSent()
        {
            // Arrange
            // Create a fake instance of IEmailService
            var fakeEmailService = A.Fake<IEmailService>();
            // Set up the fake behavior
            A.CallTo(() => fakeEmailService.SendEmailAsync(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(true));
            // Create NotificationService with fakeEmailService
            var notificationService = new NotificationService(fakeEmailService);
            var notification = new Notification
            {
                To = "user@example.com",
                Subject = "Notification",
                Body = "Test message"
            };

            // Act
            // Call the NotifyAsync method
            var result = await notificationService.NotifyAsync(notification);

            // Assert
            // Verify the response status code
            Assert.True(result);
            // Verify the email service was called correctly
            A.CallTo(() => fakeEmailService.SendEmailAsync(notification.To, notification.Subject, notification.Body))
                .MustHaveHappenedOnceExactly();
        }
    }
}
