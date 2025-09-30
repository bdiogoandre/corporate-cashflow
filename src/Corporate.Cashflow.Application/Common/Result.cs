using Microsoft.AspNetCore.Mvc;

namespace Corporate.Cashflow.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public List<string> Errors { get; private set; } = new();

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
        public static Result<T> Failure(params string[] errors) => new Result<T> { IsSuccess = false, Errors = errors.ToList() };

        public IActionResult ToActionResult()
        {
            if (IsSuccess)
                return new OkObjectResult(Value);

            return new BadRequestObjectResult(new { Errors });
        }
    }

}
