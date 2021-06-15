using System;
using System.Collections.Generic;
using System.Text;

namespace Notejam.Api.Business
{
    public class NoteModel
    {
        public string Id { get; set; }
        public string NoteName { get; set; }
        public string NoteText { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string PadType { get; set; }
        public string UserId { get; set; }
        public string DocumentTtpe { get; set; }
    }
}
