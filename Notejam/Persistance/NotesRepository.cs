using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notejam.Api.Business;

namespace Notejam.Api.Persistance
{
    public class NotesRepository : INotesRepository
    {
        private readonly IDocumentRepository<NotesDocumentModel> _documentDb;
        public NotesRepository(IDocumentRepository<NotesDocumentModel> documentDb)
        {
            _documentDb = documentDb;
        }
        public async Task CreateNoteAsync(NotesDocumentModel notesDocument)
        {
            string id = await _documentDb.CreateAsync(notesDocument).ConfigureAwait(false);
        }

        public async Task<IEnumerable<NotesDocumentModel>> GetAllNotes(string query)
        {
            var notesDocument = await _documentDb.GetAsync(query).ConfigureAwait(false);
            return notesDocument;
        }
    }
}
