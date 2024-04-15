using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class Result
    {
        private Result(bool isSuccess, IEnumerable<string>? errors)
        {
            if (isSuccess && errors != null ||
                !isSuccess && errors == null)
            {
                throw new ArgumentException("Invalid error", nameof(errors));
            }

            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IEnumerable<string>? Errors { get; }

        public static Result Success() => new(true, default);
        public static Result Failure(IEnumerable<string> errors) => new(false, errors);
    }

    public class Result<TValue>
    {
        public TValue? Value { get; init; }
        private Result(bool isSuccess, TValue? value, IEnumerable<string>? errors)
        {
            if (isSuccess && errors != null && value == null)
            {
                throw new ArgumentException("Errors should be null for successful result", nameof(errors));
            } 
            else if (!isSuccess && errors == null && value != null)
            {
                throw new ArgumentException("Errors should not be null for failed result", nameof(errors));
            }

            Value = value;
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IEnumerable<string>? Errors { get; }

        public static Result<TValue> Success(TValue value) => new(true, value, null);
        public static Result<TValue> Failure(IEnumerable<string> errors) => new(false, default, errors);
    }
}
