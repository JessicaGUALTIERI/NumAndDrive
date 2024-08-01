using System.Text.RegularExpressions;
using NumAndDrive.Models.Repositories;
using NumAndDrive.Services;

namespace NumAndDrive.UnitTests;

[TestClass]
public class AdminServiceTests
{
    AdminService fileDataTest;

    [TestInitialize]
    public void Init()
    {
        fileDataTest = new AdminService();
    }

    [TestMethod]
    public void PasswordIsValid()
    {
        // Arrange
        string password = fileDataTest.PasswordGenerator();

        // Act
        bool result = Regex.IsMatch(password, "[A-Z]")
            && Regex.IsMatch(password, "[a-z]")
            && Regex.IsMatch(password, @"[\d]")
            && Regex.IsMatch(password, @"[!@#$%^&*()_+]")
            && password.Length >= 8
            && password.Length <= 12;

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneUppercase()
    {
        // Arrange
        string password = fileDataTest.PasswordGenerator();

        // Act
        bool result = Regex.IsMatch(password, "[A-Z]");

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneLowercase()
    {
        // Arrange
        string password = fileDataTest.PasswordGenerator();

        // Act
        bool result = Regex.IsMatch(password, "[a-z]");

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneDigit()
    {
        // Arrange
        string password = fileDataTest.PasswordGenerator();

        // Act
        bool result = Regex.IsMatch(password, @"[\d]");

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneNonAlphanumericCharacter()
    {
        // Arrange
        string password = fileDataTest.PasswordGenerator();

        // Act
        bool result = Regex.IsMatch(password, @"[!@#$%^&*()_+]");

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void PasswordHasTheCorrectLength()
    {
        // Arrange
        string password = fileDataTest.PasswordGenerator();

        // Act
        bool result = password.Length >= 8 && password.Length <= 12;

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("Jessica GUALTIERI")]
    [DataRow("MATHIAS d          ")]
    [DataRow("          cyril C")]
    [DataRow("Jeanne d'Arc")]
    public void NameIsCorrect_UsingCorrectInput_ReturnTrue(string name)
    {
        // Act
        bool result = fileDataTest.IsNameValid(name);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("X Æ A-12")]
    [DataRow("J. Jacques")]
    [DataRow("")]
    [DataRow("               ")]
    public void NameIsCorrect_UsingWrongInput_ReturnFalse(string name)
    {
        // Act
        bool result = fileDataTest.IsNameValid(name);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow("jessica@gmail.com")]
    [DataRow("    jess123@live.fr")]
    [DataRow("j.e.s.s_123@live.fr   ")]
    public void EmailIsCorrect_UsingCorrectInput_ReturnTrue(string email)
    {
        // Act
        bool result = fileDataTest.IsEmailValid(email);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("_jessica@gmail.com")]
    [DataRow("jess123@live.f")]
    [DataRow("@live.fr")]
    [DataRow("jessica.live.fr")]
    [DataRow("jessica@live")]
    [DataRow("")]
    [DataRow("               ")]
    public void EmailIsCorrect_UsingWrongInput_ReturnFalse(string email)
    {
        // Act
        bool result = fileDataTest.IsEmailValid(email);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow("      0606060606")]
    [DataRow("06 06 06 06 06")]
    [DataRow("06         06          06 06 06")]
    [DataRow("0000000000")]
    public void PhoneNumberIsCorrect_UsingCorrectInput_ReturnTrue(string phoneNumber)
    {
        // Act
        bool result = fileDataTest.IsPhoneNumberValid(phoneNumber);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("06")]
    [DataRow("+33 4 04 04 04 04")]
    [DataRow("")]
    [DataRow("               ")]
    public void PhoneNumberIsCorrect_UsingWrongInput_ReturnFalse(string phoneNumber)
    {
        // Act
        bool result = fileDataTest.IsPhoneNumberValid(phoneNumber);

        // Assert
        Assert.IsFalse(result);
    }
}
