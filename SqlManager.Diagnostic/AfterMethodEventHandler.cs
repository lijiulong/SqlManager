namespace Franksoft.SqlManager.Diagnostic
{
    /// <summary>
    /// Represents the method that will handle events when a normal method is executed and returned.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="AfterEventArgs"/> instance that contains the event data.</param>
    public delegate void AfterMethodEventHandler(object sender, AfterEventArgs e);
}
