using System;

namespace GUISupport.Services
{
    public class TestFileInteractionService : IFileInteractionService
    {
        public FileInteractionResult OpenFile(string defaultExt, string initialDirectory = null, string filter = null)
        {
            throw new NotImplementedException();
        }

        public FileInteractionResult SaveFile(string defaultExt, string fileName, string initialDirectory = null, string filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
