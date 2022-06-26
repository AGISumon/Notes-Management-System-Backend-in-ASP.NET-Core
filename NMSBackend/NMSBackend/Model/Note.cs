using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NMSBackend.Model
{
    public class Note
    {
        public int NoteId { get; set; }
        public string NoteType { get; set; }
        [MaxLength(100)]
        public string NoteDetails { get; set; }
        public DateTime? Time { get; set; }
        public string BookmarkUrl { get; set; }
        public string Status { get; set; }
    }
}
