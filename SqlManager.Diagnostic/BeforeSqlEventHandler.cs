namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// Represents the method that will handle events when a sql related method is called before execution.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="BeforeSqlEventArgs"/> instance that contains the event data.</param>
    public delegate void BeforeSqlEventHandler(object sender, BeforeSqlEventArgs e);
}
