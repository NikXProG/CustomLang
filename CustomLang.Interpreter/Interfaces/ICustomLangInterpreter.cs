namespace CustomLang.Interpreter.interfaces;


/// <summary>
/// Defines a code interpreter interface for parsing and executing source code
/// </summary>
public interface ICodeInterpreter
{
    /// <summary>
    /// Asynchronously parses and executes the provided source code
    /// </summary>
    /// <param name="code">The source code to parse and execute</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the parsing operation
    /// </param>
    /// <returns>A task that represents the asynchronous parse operation</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="code"/> is null
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled via the cancellation token
    /// </exception>
    /// <remarks>
    /// The asynchronous version is preferred for long-running parsing operations
    /// as it doesn't block the calling thread and supports cancellation.
    /// </remarks>
    Task ParseAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronously parses and executes the provided source code
    /// </summary>
    /// <param name="code">The source code to parse and execute</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="code"/> is null
    /// </exception>
    /// <remarks>
    /// This synchronous version is suitable for simple or short-running parsing operations.
    /// For complex parsing, consider using <see cref="ParseAsync"/> instead.
    /// </remarks>
    void Parse(string code);
}