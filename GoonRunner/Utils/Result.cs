namespace GoonRunner.Utils
{
    /// <summary>
    /// Represents the result of an operation that can either succeed or fail.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value when the operation succeeds.</typeparam>
    /// <typeparam name="TFailure">The type of the error value when the operation fails.</typeparam>
    public class Result<TSuccess, TFailure>
    {
        public bool IsSuccess { get; }

        public TSuccess Value { get; }

        public TFailure Error { get; }

        private Result(bool isSuccess, TSuccess value, TFailure error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        /// <summary>
        /// Creates a successful result with the specified value.
        /// </summary>
        /// <param name="value">The value representing the successful result.</param>
        /// <returns>A new <see cref="Result{TSuccess, TFailure}"/> indicating success.</returns>
        public static Result<TSuccess, TFailure> Success(TSuccess value) =>
            new Result<TSuccess, TFailure>(true, value, default(TFailure));

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error representing the failure result.</param>
        /// <returns>A new <see cref="Result{TSuccess, TFailure}"/> indicating failure.</returns>
        public static Result<TSuccess, TFailure> Failure(TFailure error) =>
            new Result<TSuccess, TFailure>(false, default(TSuccess), error);
    }
}
