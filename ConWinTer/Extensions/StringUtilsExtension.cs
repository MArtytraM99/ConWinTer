using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ConWinTer.Extensions {
    public static class StringUtilsExtension {
        public static bool IsValidEmailAddress(this string s) {
            try {
                var m = new MailAddress(s);
                return true;
            } catch (FormatException) {
                return false;
            }
        }
    }
}
