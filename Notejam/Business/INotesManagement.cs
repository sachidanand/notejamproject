using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notejam.Api.Persistance;

namespace Notejam.Api.Business
{
    public interface INotesManagement
    {
        void CreateNote(NoteModel noteDto);
        Task<IEnumerable<NoteModel>> GetAllNotesAsync(string query);
        void UpdateNote();
        void GetNoteById();
    }
}
