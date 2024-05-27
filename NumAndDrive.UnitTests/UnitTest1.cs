using System.Text.RegularExpressions;
using NumAndDrive.Models.Repositories;

namespace NumAndDrive.UnitTests;

[TestClass]
public class UnitTest1
{
    AdminRepository fileDataTest = new AdminRepository();

    [TestMethod]
    public void PasswordIsValid()
    {
        string password = fileDataTest.PasswordGenerator();

        Assert.IsTrue(Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, @"[\d]") && Regex.IsMatch(password, @"[!@#$%^&*()_+]") && password.Length >= 8 && password.Length <= 12);
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneUppercase()
    {
        string password = fileDataTest.PasswordGenerator();
        Assert.IsTrue(Regex.IsMatch(password, "[A-Z]"));
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneLowercase()
    {
        string password = fileDataTest.PasswordGenerator();
        Assert.IsTrue(Regex.IsMatch(password, "[a-z]"));
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneDigit()
    {
        string password = fileDataTest.PasswordGenerator();
        Assert.IsTrue(Regex.IsMatch(password, @"[\d]"));
    }

    [TestMethod]
    public void PasswordContainsAtLeastOneNonAlphanumericCharacter()
    {
        string password = fileDataTest.PasswordGenerator();
        Assert.IsTrue(Regex.IsMatch(password, @"[!@#$%^&*()_+]"));
    }

    [TestMethod]
    public void PasswordHasTheCorrectLength()
    {
        string password = fileDataTest.PasswordGenerator();
        Assert.IsTrue(password.Length >= 8 && password.Length <= 12);
    }

    [TestMethod]
    [DataRow("Jessica GUALTIERI")]
    [DataRow("MATHIAS d          ")]
    [DataRow("          cyril C")]
    [DataRow("Jeanne d'Arc")]
    public void NameIsCorrect_UsingCorrectInput_ReturnTrue(string name)
    {
        bool result = fileDataTest.IsNameValid(name);
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("X Æ A-12")]
    [DataRow("J. Jacques")]
    [DataRow("")]
    [DataRow("               ")]
    public void NameIsCorrect_UsingWrongInput_ReturnFalse(string name)
    {
        bool result = fileDataTest.IsNameValid(name);
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow("jessica@gmail.com")]
    [DataRow("    jess123@live.fr")]
    [DataRow("j.e.s.s_123@live.fr   ")]
    public void EmailIsCorrect_UsingCorrectInput_ReturnTrue(string email)
    {
        bool result = fileDataTest.IsEmailValid(email);
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
        bool result = fileDataTest.IsEmailValid(email);
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow("      0606060606")]
    [DataRow("06 06 06 06 06")]
    [DataRow("06         06          06 06 06")]
    [DataRow("0000000000")]
    public void PhoneNumberIsCorrect_UsingCorrectInput_ReturnTrue(string phoneNumber)
    {
        bool result = fileDataTest.IsPhoneNumberValid(phoneNumber);
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("06")]
    [DataRow("+33 4 04 04 04 04")]
    [DataRow("")]
    [DataRow("               ")]
    public void PhoneNumberIsCorrect_UsingWrongInput_ReturnFalse(string phoneNumber)
    {
        bool result = fileDataTest.IsPhoneNumberValid(phoneNumber);
        Assert.IsFalse(result);
    }
}
