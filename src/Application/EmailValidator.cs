using System.Net.Mail;

namespace Application;

/// <summary>
/// A static class used to validate e-mails.
/// </summary>
public static class EmailValidator
{
    /// <summary>
    /// Static method used to validate e-mails.
    /// </summary>
    /// <param name="email">The string to be validated.</param>
    /// <returns>If the input is a valid email or not.</returns>
    public static bool IsValidEmail(string email)
    {
        string trimmedEmail = email.Trim();
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
