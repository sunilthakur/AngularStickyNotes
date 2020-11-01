using Microsoft.EntityFrameworkCore;
using StickyNotesAPIService.Common;
using StickyNotesAPIService.Data;
using StickyNotesAPIService.Entities;
using StickyNotesAPIService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace StickyNotesAPIService.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly NotesDbContext context;
        public NotesRepository(NotesDbContext notesContext)
        {
            context = notesContext ?? throw new ArgumentNullException(nameof(notesContext));
        }
        public async Task<ActionOutput<Notes>> CreateNote(Notes note)
        {
            note.CreatedOn = DateTime.Now;
            note.UpdatedOn = DateTime.Now;
            await context.StickyNotes.AddAsync(note);
            await context.SaveChangesAsync();
            return new ActionOutput<Notes>()
            {
                Message = "Sticky Note has been created successfully",
                Status = ActionStatus.Successfull,
                Object = note
            };
        }

        public async Task<ActionOutput<int>> DeleteNote(int noteId)
        {
            var note = await context.StickyNotes.FindAsync(noteId);
            context.StickyNotes.Remove(note);
            await context.SaveChangesAsync();
            return new ActionOutput<int>()
            {
                Status = ActionStatus.Successfull,
                Message = "Sticky Note has been removed successfully",
                Object = 1
            };
        }

        public async Task<PagingResult<Notes>> GetNotesByUser(PagingModel pagingModel)
        {
            var result = new PagingResult<Notes>();
            var notes = context.StickyNotes.AsNoTracking().Where(x => x.UserId == pagingModel.UserId).OrderBy(pagingModel.SortBy + " " + pagingModel.SortOrder); ;
            var list = await notes
              .Skip(pagingModel.PageNo - 1).Take(pagingModel.RecordsPerPage)
              .ToListAsync();
            result.List = list;
            result.Status = ActionStatus.Successfull;
            result.Message = "Sticky Notes List";
            result.TotalCount = notes.Count();
            return result;
        }

        public async Task<ActionOutput<Notes>> UpdateNote(Notes note)
        {
            var result = new ActionOutput<Notes>();
            var existingNote = await context.StickyNotes.FindAsync(note.NoteId);
            if (existingNote != null)
            {
                existingNote.NoteTitle = note.NoteTitle;
                existingNote.NoteBody = note.NoteBody;
                existingNote.NoteFooter = note.NoteFooter;
                existingNote.UpdatedOn = DateTime.Now;
                existingNote.NoteForeGroundColor = note.NoteForeGroundColor;
                existingNote.NoteBackGroundColor = note.NoteBackGroundColor;
                await context.SaveChangesAsync();
                result.Status = ActionStatus.Successfull;
                result.Message = "Sticky Note has been updated successfully";
                result.Object = existingNote;
            }
            else
            {
                result.Status = ActionStatus.Error;
                result.Message = "Sticky Note has not been updated";
            }
            return result;
        }
    }
}
