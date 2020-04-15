namespace GUISupport.Services
{
    /// <summary>
    /// Interface for interacting with the file structure.
    /// </summary>
    public interface IFileInteractionService
    {
        /// <summary>
        /// Interaction with a saving dialog.
        /// </summary>
        FileInteractionResult SaveFile(string defaultExt, string fileName, string initialDirectory = null, string filter = null);

        /// <summary>
        /// Interaction with an opening file dialog.
        /// </summary>
        FileInteractionResult OpenFile(string defaultExt, string initialDirectory = null, string filter = null);
    }
}
