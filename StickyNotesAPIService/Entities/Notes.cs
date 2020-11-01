using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StickyNotesAPIService.Entities
{
    public class Notes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteBody { get; set; }
        public string NoteFooter { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UserId { get; set; }
        public string NoteBackGroundColor { get; set; }
        public string NoteForeGroundColor { get; set; }
    }
}
