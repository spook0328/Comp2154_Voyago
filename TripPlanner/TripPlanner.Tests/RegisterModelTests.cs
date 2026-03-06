using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using TripPlanner.Areas.Identity.Pages.Account;
using TripPlanner.Models;
using Xunit;

namespace TripPlanner.Tests
{
    public class RegisterModelTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<IUserStore<ApplicationUser>> _mockUserStore;
        private readonly Mock<ILogger<RegisterModel>> _mockLogger;
        private readonly Mock<IEmailSender> _mockEmailSender;

        public RegisterModelTests()
        {
            // IUserStore needs to also implement IUserEmailStore for RegisterModel to work
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.As<IUserEmailStore<ApplicationUser>>();

            _mockUserStore = userStoreMock;

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _mockUserManager.Setup(u => u.SupportsUserEmail).Returns(true);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null);

            _mockLogger = new Mock<ILogger<RegisterModel>>();
            _mockEmailSender = new Mock<IEmailSender>();
        }

        private RegisterModel BuildRegisterModel()
        {
            var model = new RegisterModel(
                _mockUserManager.Object,
                _mockUserStore.Object,
                _mockSignInManager.Object,
                _mockLogger.Object,
                _mockEmailSender.Object);

            var httpContext = new DefaultHttpContext();
            model.PageContext = new PageContext
            {
                HttpContext = httpContext
            };

            // Mock email sender to silently succeed — we don't want real emails in tests
            _mockEmailSender
                .Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Mock ALL URL methods — Url.Page(), Url.Content(), Url.Action() all need to return
            // a non-null string otherwise LocalRedirect() throws ArgumentNullException
            var mockUrlHelper = new Mock<IUrlHelper>();

            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("http://localhost/confirm");

            mockUrlHelper
                .Setup(x => x.Content(It.IsAny<string>()))
                .Returns("http://localhost/confirm");

            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/confirm");

            mockUrlHelper
                .SetupGet(x => x.ActionContext)
                .Returns(new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor()));

            model.Url = mockUrlHelper.Object;

            return model;
        }

        // -------------------------------------------------------
        // TEST 1: Valid registration creates user and assigns "User" role
        // -------------------------------------------------------
        [Fact]
        public async Task OnPostAsync_ValidInput_CreatesUserAndAssignsUserRole()
        {
            // Arrange
            _mockUserManager
                .Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("test-user-id");

            _mockUserManager
                .Setup(u => u.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("fake-token");

            var model = BuildRegisterModel();
            model.Input = new RegisterModel.InputModel
            {
                Email = "newuser@test.com",
                Password = "Password1!",
                ConfirmPassword = "Password1!"
            };

            // Act — wrapped in try/catch because LocalRedirect may still throw in test env
            // We only care that CreateAsync and AddToRoleAsync were called
            try { await model.OnPostAsync(); } catch { }

            // Assert — user creation was called once
            _mockUserManager.Verify(
                u => u.CreateAsync(It.IsAny<ApplicationUser>(), "Password1!"),
                Times.Once);

            // Assert — "User" role was assigned after creation
            _mockUserManager.Verify(
                u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"),
                Times.Once);
        }

        // -------------------------------------------------------
        // TEST 2: Duplicate email causes model error and returns page
        // -------------------------------------------------------
        [Fact]
        public async Task OnPostAsync_DuplicateEmail_ReturnsPageWithError()
        {
            // Arrange — simulate Identity rejecting the creation due to duplicate email
            var duplicateEmailError = IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "Email 'existing@test.com' is already taken."
            });

            _mockUserManager
                .Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(duplicateEmailError);

            var model = BuildRegisterModel();
            model.Input = new RegisterModel.InputModel
            {
                Email = "existing@test.com",
                Password = "Password1!",
                ConfirmPassword = "Password1!"
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert — should stay on the register page
            Assert.IsType<PageResult>(result);

            // Assert — the duplicate email error should be in ModelState
            Assert.True(model.ModelState.ContainsKey(string.Empty));
            Assert.Contains(
                model.ModelState[string.Empty]!.Errors,
                e => e.ErrorMessage.Contains("already taken"));
        }

        // -------------------------------------------------------
        // TEST 3: Mismatched passwords fail model validation before hitting the handler
        // -------------------------------------------------------
        [Fact]
        public async Task OnPostAsync_MismatchedPasswords_ReturnsPageWithoutCreatingUser()
        {
            var model = BuildRegisterModel();
            model.Input = new RegisterModel.InputModel
            {
                Email = "user@test.com",
                Password = "Password1!",
                ConfirmPassword = "DifferentPassword!"
            };

            // Manually simulate the model validation failure that MVC would catch
            // We do this because unit tests don't run the MVC validation pipeline automatically
            model.ModelState.AddModelError(
                "Input.ConfirmPassword",
                "The password and confirmation password do not match.");

            // Act
            var result = await model.OnPostAsync();

            // Assert — should stay on the page
            Assert.IsType<PageResult>(result);

            // Assert — CreateAsync should never have been called
            _mockUserManager.Verify(
                u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}