using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Notejam.Api.Persistance
{
    public class NotesDocumentModel
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }
        [DataMember]
        [JsonProperty("noteText")]
        public string NoteText { get; set; }
        [DataMember]
        [JsonProperty("noteName")]
        public string NoteName { get; set; }
        [DataMember]
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }
        [DataMember]
        [JsonProperty("padType")]
        public string PadType { get; set; }
        [DataMember]
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [DataMember]
        [JsonProperty("_type")]
        public string DocumentTtpe { get; set; } = "NotesContent";
    }
}
