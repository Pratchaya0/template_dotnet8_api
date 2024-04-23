using Newtonsoft.Json;
using Serilog;
using System.IO;
using template_dotnet8_api.DTOs.Pagination;
using template_dotnet8_api.Exceptions;

namespace template_dotnet8_api.Models
{
    public class ServiceResponse<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public T Data { get; set; }

        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = null;

        public int? Code { get; set; }
        public object ExceptionMessage { get; set; } = null;
        public DateTime ServiceDateTime { get; set; } = DateTime.Now;

        public double? TotalAmountRecords { get; set; }
        public double? TotalAmountPages { get; set; }
        public double? CurrentPage { get; set; }
        public double? RecordsPerPage { get; set; }
        public int? PageIndex { get; set; }
    }

    public static class ResponseResult
    {

        #region Success
        public static ServiceResponse<T> Success<T>(T data, PaginationResultDto paginationResult, string message = "Success.")
        {
            Log.Information(message);

            return new ServiceResponse<T>
            {
                Data = data,
                Message = message,
                TotalAmountRecords = paginationResult.TotalAmountRecords,
                TotalAmountPages = paginationResult.TotalAmountPages,
                CurrentPage = paginationResult.CurrentPage,
                RecordsPerPage = paginationResult.RecordsPerPage,
                PageIndex = paginationResult.PageIndex,
            };
        }

        public static ServiceResponse<T> Success<T>(T data, PaginationResultDto paginationResult)
        {
            Log.Information("Success");

            return new ServiceResponse<T>
            {
                Data = data,
                TotalAmountRecords = paginationResult.TotalAmountRecords,
                TotalAmountPages = paginationResult.TotalAmountPages,
                CurrentPage = paginationResult.CurrentPage,
                RecordsPerPage = paginationResult.RecordsPerPage,
                PageIndex = paginationResult.PageIndex,
            };
        }

        public static ServiceResponse<T> Success<T>(T data, string message = "Success.")
        {
            Log.Information(message);

            return new ServiceResponse<T>
            {
                Data = data,
                Message = message,
            };
        }

        public static ServiceResponse<T> Success<T>(T data)
        {
            Log.Information("Success");

            return new ServiceResponse<T>
            {
                Data = data
            };
        }

        #endregion Success

        #region Error

        public static ServiceResponse<T> Failure<T>(string message, int? errorCode = null, object exceptionMessage = null) where T : class
        {
            return new ServiceResponse<T>
            {
                Data = null,
                IsSuccess = true,
                Message = message,
                Code = errorCode,
                ExceptionMessage = exceptionMessage
            };
        }

        public static ServiceResponse<T> NotFound<T>(string objectTypeName) where T : class
        {
            throw new NotFoundException(objectTypeName);
        }

        public static ServiceResponse<T> InvalidGUID<T>(string objectTypeName, string keys) where T : class
        {
            throw new InvalidGUIDException(objectTypeName, keys);
        }

        public static ServiceResponse<T> InvalidDate<T>(string objectTypeName, string keys) where T : class
        {
            throw new InvalidDateException(objectTypeName, keys);
        }

        public static ServiceResponse<T> Null<T>(string objectTypeName) where T : class
        {
            throw new NullException(objectTypeName);
        }

        #endregion Error
    }
}
