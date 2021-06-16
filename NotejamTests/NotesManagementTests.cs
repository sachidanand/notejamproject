using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notejam.Api.Business;
using Notejam.Api.Persistance;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotejamTests
{
    [TestClass]
    public class NotesManagementTests
    {
        private Mock<INotesRepository> _notesRepositoryMock;
        private INotesManagement _notesManagement;

        [TestInitialize]
        public void InitUsers()
        {
            _notesRepositoryMock = new Mock<INotesRepository>();
            _notesManagement = new NotesManagement(_notesRepositoryMock.Object);
        }

            [TestMethod]
        public async Task GetAllNotes_WithoutFilter_ReturnListOfNotesAsync()
        {
            var lstNotes = new List<NotesDocumentModel>();
            for (var i=1; i<=2; i++)
            {
                var notesDocumentModel = new NotesDocumentModel
                {
                    Id = Guid.NewGuid().ToString(),
                    NoteName = "TestNote1",
                    NoteText = "Test",
                    PadType = "Personal",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    UserId = "23"
                };
                lstNotes.Add(notesDocumentModel);
            }
            
            _notesRepositoryMock.Setup(x => x.GetAllNotes("SELECT * FROM Table")).Returns(Task.FromResult<IEnumerable<NotesDocumentModel>>(lstNotes));
            var result = await _notesManagement.GetAllNotesAsync("SELECT * FROM Table").ConfigureAwait(false);
            Assert.AreNotEqual(result, null);
        }
    }
}
