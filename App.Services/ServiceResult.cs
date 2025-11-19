using System.Net;

namespace App.Services {
    public class ServiceResult<T> {
#nullable enable
        public T? Data { get; set; }
        public List<string>? ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
        public HttpStatusCode StatusCode { get; set; }

        public static ServiceResult<T> Success(HttpStatusCode statusCode = HttpStatusCode.OK) {
            return new ServiceResult<T> {
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK) {
            return new ServiceResult<T> {
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            return new ServiceResult<T> {
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            return new ServiceResult<T> {
                ErrorMessage = [errorMessage],
                StatusCode = statusCode
            };
        }
    }

    public class ServiceResult {
#nullable enable
        public List<string>? ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
        public HttpStatusCode StatusCode { get; set; }

        public static ServiceResult Success(HttpStatusCode statusCode = HttpStatusCode.OK) {
            return new ServiceResult {
                StatusCode = statusCode
            };
        }

        public static ServiceResult Fail(List<string> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            return new ServiceResult {
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }

        public static ServiceResult Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            return new ServiceResult {
                ErrorMessage = [errorMessage],
                StatusCode = statusCode
            };
        }
    }
}