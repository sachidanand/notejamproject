using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notejam.Api.Persistance
{
    public interface INotesRepository
    {
        Task CreateNoteAsync(NotesDocumentModel notesDocument);
        Task<IEnumerable<NotesDocumentModel>> GetAllNotes(string query);
    }
}
