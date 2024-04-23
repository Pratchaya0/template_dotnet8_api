namespace template_dotnet8_api.Exceptions
{
    public class NotFoundException : AppExceptionBase
    {
        public NotFoundException(string objectTypeName)
        {
            ObjectTypeName = objectTypeName;
        }

        public override string Message => $"Object [{ObjectTypeName}] is not found.";
    }
}
