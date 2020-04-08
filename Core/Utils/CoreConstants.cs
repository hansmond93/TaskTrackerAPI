namespace Core.Utils
{
    public abstract class CoreConstants
    {

        public const string AdminUser = "administrator@trackR.com";
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";
        public const string SkuPrefix = "SKU-";
        public const int requestIdNoMinLength = 8;

        public static readonly string[] validExcels = new[] { ".xls", ".xlsx" };

        public class MailUrl
        {
            public const string PasswordReset = "filestore/emailtemplates/passwordreset.htm";
        }

        public class PaginationConsts
        {
            public const int PageSize = 25;
            public const int PageIndex = 1;
        }

        public class ClaimsKey
        {
            public const string LastLogin = nameof(LastLogin);
            public const string Division = nameof(Division);
            public const string Function = nameof(Function);
            public const string Grade = nameof(Grade);
            public const string Branch = nameof(Branch);
            public const string Directorate = nameof(Directorate);
            public const string Region = nameof(Region);
            public const string Unit = nameof(Unit);
            public const string JobCategory = nameof(JobCategory);
            public const string Permissions = nameof(Permissions);
        }

        public class AllowedFileExtensions
        {
            public const string Signature = ".jpg,.png";
        }

        public class JobFunction
        {
            public const string DH = "Divisional Head";
            public const string BM = "Branch Manager";
            public const string ED = "Executive Director";
            public const string MD = "Managing Director";
            public const string RBH = "Regional Bank Head";
            public const string BO = "Banking Officer";
        }

        public class Dashboard
        {

            public static string[] Months = new string[] {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
        }
    }
}