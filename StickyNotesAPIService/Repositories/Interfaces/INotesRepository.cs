using StickyNotesAPIService.Common;
using StickyNotesAPIService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickyNotesAPIService.Repositories.Interfaces
{
    public interface INotesRepository
    {
        Task<PagingResult<Notes>> GetNotesByUser(PagingModel pagingModel);
        Task<ActionOutput<Notes>> CreateNote(Notes note);
        Task<ActionOutput<Notes>> UpdateNote(Notes note);
        Task<ActionOutput<int>> DeleteNote(int noteId);
    }
}
