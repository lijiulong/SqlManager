namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// Represents the method that will handle events when a normal method is called before execution.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="BeforeEventArgs"/> instance that contains the event data.</param>
    public delegate void BeforeMethodEventHandler(object sender, BeforeEventArgs e);
}
