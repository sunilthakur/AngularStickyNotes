using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StickyNotesAPIService.Common;
using StickyNotesAPIService.Entities;
using StickyNotesAPIService.Repositories.Interfaces;

namespace StickyNotesAPIService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository notesRepository;
        private readonly ILogger<NotesController> _logger;
        public NotesController(INotesRepository repository, ILogger<NotesController> logger)
        {
            notesRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpPost]
        [Route("createnote")]
        public async Task<ActionOutput<Notes>> CreateNote([FromBody] Notes note)
        {
            return await notesRepository.CreateNote(note);
        }

        [HttpPost]
        [Route("getnotespagedlist")]
        public async Task<PagingResult<Notes>> GetNotesPagedList([FromBody] PagingModel model)
        {
            model.SortBy = "NoteId";
            model.SortOrder = "Desc";
            return await notesRepository.GetNotesByUser(model);
        }

        [HttpPut]
        [Route("updatenote")]
        public async Task<ActionOutput<Notes>> UpdateNote([FromBody] Notes note)
        {
            return await notesRepository.UpdateNote(note);
        }
        [HttpDelete]
        [Route("deletenote")]
        public async Task<ActionOutput<int>> DeleteNote([FromQuery]int noteId)
        {
            return await notesRepository.DeleteNote(noteId);
        }
    }
}
