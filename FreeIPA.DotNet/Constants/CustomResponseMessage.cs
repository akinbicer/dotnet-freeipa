namespace FreeIPA.DotNet.Constants;

public class CustomResponseMessage
{
    public static string RecordAdded = "RECORD_ADDED";
    public static string RecordUpdated = "RECORD_UPDATED";
    public static string RecordDeleted = "RECORD_DELETED";
    public static string RecordNotFound = "RECORD_NOT_FOUND";
    public static string RecordAlreadyExists = "RECORD_ALREADY_EXISTS";

    public static string PasswordChangeSuccess = "PASSWORD_CHANGE_SUCCESS";
    public static string PasswordChangeFailed = "PASSWORD_CHANGE_FAILED";

    public static string SuccessfulLogin = "SUCCESS_FULL_LOGIN";
    public static string FailedLogin = "FAILED_LOGIN";

    public static string TransactionSuccess = "TRANSACTION_SUCCESS";
    public static string TransactionError = "TRANSACTION_ERROR";
}