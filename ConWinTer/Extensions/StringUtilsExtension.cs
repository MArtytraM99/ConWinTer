using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ConWinTer.Extensions {
    public static class StringUtilsExtension {
        public static bool IsValidEmailAddress(this string s) {
            if (string.IsNullOrWhiteSpace(s))
                return false;

            try {
                var m = new MailAddress(s);
                return true;
            } catch (FormatException) {
                return false;
            }
        }
    }
}
