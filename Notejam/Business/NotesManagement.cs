using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notejam.Api.Persistance;

namespace Notejam.Api.Business
{
    internal class NotesManagement : INotesManagement
    {

        private readonly INotesRepository _document;
        public NotesManagement(INotesRepository document)
        {
            _document = document;
        }
        public void CreateNote(NoteModel noteDto)
        {
            var documentModel = new NotesDocumentModel
            {
                Id = Guid.NewGuid().ToString(),
                NoteName = noteDto.NoteName,
                NoteText = noteDto.NoteText,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                PadType = noteDto.PadType,
                UserId = noteDto.UserId
            };
            _document.CreateNoteAsync(documentModel);
        }

        public void GetNoteById()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NoteModel>> GetAllNotesAsync(string query)
        {
            var lstNotes = new List<NoteModel>();
            var notesDocument = await _document.GetAllNotes(query).ConfigureAwait(false);

            foreach (var document in notesDocument)
            {
                var noteModel = new NoteModel
                {
                    Id = document.Id,
                    NoteName = document.NoteName,
                    NoteText = document.NoteText,
                    PadType = document.PadType,
                    CreatedDate = document.CreatedDate,
                    LastModifiedDate = document.LastModifiedDate,
                    UserId = document.UserId
                };
                lstNotes.Add(noteModel);

            }

            return lstNotes;
        }

        public void UpdateNote()
        {
            throw new NotImplementedException();
        }
    }
}
