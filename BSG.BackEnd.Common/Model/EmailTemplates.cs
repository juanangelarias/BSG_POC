namespace BSG.BackEnd.Common.Model;

public static class EmailTemplates
{
    public const string WelcomeTemplate =
        "<p>" +
        "    <img src=\"&&LOGO_URL&&\" alt=\"\" width=\"161\" height=\"47\" />" +
        "</p>" +
        "<p>&nbsp;</p>" +
        "<p>&&FULL_NAME&&,</p>" +
        "<p>Welcome to App Portal!</p>" +
        "<p style=\"margin-top:25px;\">" +
        "    Sign in to your account using the following credentials:" +
        "</p>" +
        "<p style=\"padding-left: 40px; margin-top:-10px;\">" +
        "    Email: &&EMAIL_ADDRESS&&" +
        "</p>" +
        "<p style=\"margin-top:25px;\">" +
        "    Upon the first login, it will prompt you to set up your password." +
        "</p>" +
        "<table style=\"border-collapse: collapse; width: 175px; height: 50px; background-color:#009641\" border=\"0\">" +
        "    <tbody>" +
        "        <tr style=\"height: 54px;\">" +
        "            <td style=\"width: 100%; text-align: center; height: 54px;\">" +
        "                <h3>" +
        "                    <strong><a style=\"color: #ffffff;\" href=\"&&LINK_URL&&\">Login</a></strong>" +
        "                </h3>" +
        "            </td>" +
        "        </tr>" +
        "    </tbody>" +
        "</table>";

    public const string ResetPasswordTemplate =
        "<p>" +
        "    <img src=\"&&LOGO_URL&&\" alt=\"\" width=\"161\" height=\"47\" />" +
        "</p>" +
        "<p>&nbsp;</p>" +
        "<p>&&FULL_NAME&&,</p>" +
        "<p style=\"margin-top: 25px;\">" +
        "    Follow this link to reset your password for App Portal." +
        "</p>" +
        "<p style=\"margin-top: 25px;\">" +
        "    If you did not request a new password, you can safely delete this email." +
        "</p>" +
        "<table style=\"border-collapse: collapse; width: 350px; height: 50px;\" border=\"0\">" +
        "    <tbody>" +
        "        <tr style=\"height: 54px;\">" +
        "            <td style=\"width: 210px; text-align: center; height: 54px;  background-color: #009641;\">" +
        "                <h3>" +
        "                    <strong><a style=\"color: #ffffff;\" href=\"&&LINK_URL&&\">Reset your Password</a></strong>" +
        "                </h3>" +
        "            </td>" +
        "            <td style=width:10px;>" +
        "            </td>" +
        "            <td style=\"width:130px; text-align: center;\">" +
        "                Or <a href=\"$$BASE_URL$$\">Go to App Portal</a>" +
        "            </td>" +
        "        </tr>" +
        "    </tbody>" +
        "</table>";

    public const string PasswordChangedConfirmationTemplate =
        $"<p>" +
        $"    <img src=\"&&LOGO_URL&&\" alt=\"\" width=\"161\" height=\"47\" />" +
        $"</p>" +
        $"<p>&nbsp;</p>" +
        $"<p>&&FULL_NAME&&,</p>" +
        $"<p style=\"margin-top: 25px;\">" +
        $"     You have successfully changed your password." +
        $"</p>" +
        $"<p style=\"margin-top: 25px;\">" +
        $"     If you did not request a new password, follow this link to reset your password for App Portal." +
        $"</p>" +
        $"<p>&nbsp;</p>" +
        $"<table style=\"border-collapse: collapse; width: 350px; height: 50px;\" border=\"0\">" +
        $"    <tbody>" +
        $"        <tr style=\"height: 54px;\">" +
        $"            <td style=\"width: 210px; text-align: center; height: 54px;  background-color: #009641;\">" +
        $"                <h3>" +
        $"                    <strong><a style=\"color: #ffffff;\" href=\"&&LINK_URL&&\">Reset your Password</a></strong>" +
        $"                </h3>" +
        $"            </td>" +
        $"            <td style=width:10px;>" +
        $"            </td>" +
        $"            <td style=\"width:130px; text-align: center;\">" +
        $"                Or <a href=\"$$BASE_URL$$\">Go to App Portal</a>" +
        $"            </td>" +
        $"        </tr>" +
        $"    </tbody>" +
        $"</table>";
}