import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NotesService {

  constructor(private http: HttpClient) { }
  getNotesPagedList(pagingModel: any) {
    return this.http.post<any>(`${environment.serverUrl}notes/getnotespagedlist`, pagingModel);    
  }
  saveNotes(notesModel: any) {
    return this.http.post<any>(`${environment.serverUrl}notes/createnote`, notesModel);
  }
  updateNotes(notesModel: any) {
    return this.http.put<any>(`${environment.serverUrl}notes/updatenote`, notesModel);
  }
  deleteNote(noteId: number) {
    return this.http.delete<any>(`${environment.serverUrl}notes/deletenote?noteId=${noteId}`);
  }
}
