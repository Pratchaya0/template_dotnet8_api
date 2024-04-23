namespace template_dotnet8_api.Exceptions
{
    public class InvalidGUIDException : AppExceptionBase
    {
        public InvalidGUIDException(string objectTypeName, string keys)
        {
            ObjectTypeName = objectTypeName;
            Keys = keys;
        }

        public override string Message => $"Object [{ObjectTypeName}] ({Keys}) GUID is not valid.";
    }
}
