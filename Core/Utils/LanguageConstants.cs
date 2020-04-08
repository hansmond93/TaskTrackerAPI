namespace Core.Utils
{
    public static class LanguageConstants
    {
        public static class SuccessMessages
        {
            public const string Add = "Successfully Added";
            public const string Update = "Successfully Updated";
            public const string Delete = "Successfully Deleted";
        }

        public static class ErrorMessage
        {
            public const string Action = "An error occurred when processing your request";
            public const string FileUpload = "An error occurred when uploading your file. Please retry";
            public const string NoFileUploaded = "Please upload a file";
            public static readonly string BranchDoesNotExist = "Branch does not exist";
        }

        public static class Approval
        {
            public const string ApprovalLineNotFound = "No approval line created to handle this request";
            public const string ApprovalLineNameUsed = "Approval line with selected name already exists";
        }

        public static class Asset
        {
            public const string AssetNotFound = "Asset not found";
        }

        public static class AssetRequest
        {
            public const string ExceededBudget = "This request exceeds your budget, Try reducing specified quantity or make an extrabudgetary request";
            public const string InvalidIdentifier = "Invalid value for ProductId or Quantity";
            public const string FailedRequest = "Error occurred while making this request. Try again";
            public const string NoCurrentPrice = "N/A.Pls contact Procurement Unit!";
        }

        public static class Budget
        {
            public const string BudgetNotFound = "Budget not found";
        }
    }
}