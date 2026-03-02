using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using TripPlanner.Areas.Identity.Pages.Account;
using TripPlanner.Models;
using Xunit;

namespace TripPlanner.Tests
{
    public class LoginModelTests
    {
        // These are the mocked dependencies that LoginModel needs
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<LoginModel>> _mockLogger;

        public LoginModelTests()
        {
            // UserManager requires several dependencies — we mock the ones it needs
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            // SignInManager requires UserManager + several other services
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null);

            _mockLogger = new Mock<ILogger<LoginModel>>();
        }

        // Helper to build a LoginModel with a fake HTTP context attached
        private LoginModel BuildLoginModel()
        {
            var model = new LoginModel(_mockSignInManager.Object, _mockLogger.Object, _mockUserManager.Object);

            // Attach a fake PageContext so the model can process requests
            model.PageContext = new PageContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Attach a fake UrlHelper so Url.Content("~/") doesn't throw
            model.Url = new Mock<IUrlHelper>().Object;

            return model;
        }

        // -------------------------------------------------------
        // TEST 1: Valid admin credentials redirect to Admin Dashboard
        // -------------------------------------------------------
        [Fact]
        public async Task OnPostAsync_AdminUser_RedirectsToDashboard()
        {
            // Arrange
            var adminUser = new ApplicationUser { Email = "main@main.com", UserName = "main@main.com" };

            // Simulate a successful sign-in
            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Simulate finding the admin user by email
            _mockUserManager
                .Setup(u => u.FindByEmailAsync("main@main.com"))
                .ReturnsAsync(adminUser);

            // Simulate the user being in the Admin role
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(adminUser, "Admin"))
                .ReturnsAsync(true);

            var model = BuildLoginModel();
            model.Input = new LoginModel.InputModel
            {
                Email = "main@main.com",
                Password = "P@ssw0rd"
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert — should redirect to Admin Dashboard
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirect.ActionName);
            Assert.Equal("Admin", redirect.ControllerName);
        }

        // -------------------------------------------------------
        // TEST 2: Valid regular user credentials redirect to Home
        // -------------------------------------------------------
        [Fact]
        public async Task OnPostAsync_RegularUser_RedirectsToHome()
        {
            // Arrange
            var regularUser = new ApplicationUser { Email = "user@test.com", UserName = "user@test.com" };

            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockUserManager
                .Setup(u => u.FindByEmailAsync("user@test.com"))
                .ReturnsAsync(regularUser);

            // Simulate the user NOT being in the Admin role
            _mockUserManager
                .Setup(u => u.IsInRoleAsync(regularUser, "Admin"))
                .ReturnsAsync(false);

            var model = BuildLoginModel();
            model.Input = new LoginModel.InputModel
            {
                Email = "user@test.com",
                Password = "Password1!"
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert — should redirect to Home Index
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }

        // -------------------------------------------------------
        // TEST 3: Invalid credentials add a model error and return the page
        // -------------------------------------------------------
        [Fact]
        public async Task OnPostAsync_InvalidCredentials_ReturnsPageWithError()
        {
            // Arrange — simulate a failed sign-in
            _mockSignInManager
                .Setup(s => s.PasswordSignInAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var model = BuildLoginModel();
            model.Input = new LoginModel.InputModel
            {
                Email = "wrong@test.com",
                Password = "wrongpassword"
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert — should stay on the login page
            Assert.IsType<PageResult>(result);

            // Assert — should have the invalid login error message
            Assert.True(model.ModelState.ContainsKey(string.Empty));
            Assert.Contains(
                model.ModelState[string.Empty]!.Errors,
                e => e.ErrorMessage == "Invalid login attempt.");
        }
    }
}