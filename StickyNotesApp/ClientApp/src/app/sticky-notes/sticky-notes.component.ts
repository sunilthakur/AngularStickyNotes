import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { Cmyk, ColorPickerService } from 'ngx-color-picker';
import {
  AUTO_STYLE,
  animate,
  state,
  style,
  transition,
  trigger
} from '@angular/animations';
import { NotesService } from '../services/notes.service';
import { NotesModel } from '../models/user - Copy';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';

const DEFAULT_DURATION = 500;
@Component({
  selector: 'app-sticky-notes',
  //moduleId: 'src/app/sticky-notes.component',
  templateUrl: './sticky-notes.component.html',
  styleUrls: ['./sticky-notes.component.css'],
  animations: [
    trigger('collapse', [
      state('false', style({ height: AUTO_STYLE, visibility: AUTO_STYLE })),
      state('true', style({ height: '0', visibility: 'hidden' })),
      transition('false => true', animate(DEFAULT_DURATION + 'ms ease-in')),
      transition('true => false', animate(DEFAULT_DURATION + 'ms ease-out'))
    ])
  ]
})
export class StickyNotesComponent implements OnInit {
  public toggle: boolean = false;
  public rgbaText: string = 'rgba(165, 26, 214, 0.2)';

  public arrayColors: any = {
    color1: '#2883e9',
    color2: '#e920e9',
    color3: 'rgb(135 53 80)',
    color4: 'rgb(236,64,64)',
    color5: 'rgba(45,208,45,1)',
    color6: '#000'
  };
  public foreColor: string = '#fff';
  public selectedColor: string = '#000';
  public color1: string = '#2889e9';
  public color2: string = '#e920e9';
  public color3: string = 'rgb(135 53 80)';
  public color4: string = 'rgb(236,64,64)';
  public color5: string = 'rgba(45,208,45,1)';
  public color6: string = '#000';
  collapsed = true;
  public notesList: any = [];
  public notesModel: NotesModel;
  public loading: boolean = true;
  public spinner: boolean = false;
  constructor(private router: Router, public vcRef: ViewContainerRef, private cpService: ColorPickerService, private notes: NotesService, private loginService: LoginService) {
    if (!this.loginService.currentUserValue) {
      this.router.navigate(['/login']);
    }
  }
  public ngOnInit() {
    this.getnotes();
    this.notesModel = new NotesModel;
  }

  getnotes() {
    var model = {
      "pageNo": 1,
      "recordsPerPage": 999999,
      "userId": +localStorage.getItem("loggedInUserId")
    }
    this.notes.getNotesPagedList(model).subscribe(res => {
      if (res && res.status == 1) {
        this.loading = false;
        console.log("notes List=>", res);
        this.notesList = res.list;
      }
    }, error => {
      this.loading = false;
      alert('Error while fetching notes list')
    });
  }
  public onChangeColor(color: string): void {
    console.log('Color changed:', color);
    this.selectedColor = color;
    this.foreColor = '#fff';
    this.notesModel.noteForeGroundColor = '#fff';
    this.notesModel.noteBackGroundColor = color;
  }

  editNote(item: any) {
    this.setNgModel(item);
    this.collapsed = false;
  }

  setNgModel(item: any) {
    this.notesModel.noteId = item.noteId;
    this.notesModel.noteTitle = item.noteTitle;
    this.notesModel.noteBody = item.noteBody;
    this.selectedColor = item.noteBackGroundColor;
    this.notesModel.noteBackGroundColor = item.noteBackGroundColor;
    this.notesModel.noteForeGroundColor = item.noteForeGroundColor;
    this.notesModel.userId = item.userId;
  }
  onClose() {
    this.selectedColor = '#000';
    this.collapsed = true;
    this.notesModel = new NotesModel();
  }

  saveNote() {
    this.spinner = true;
    if (this.notesModel.noteId > 0) {
      this.notes.updateNotes(this.notesModel).subscribe(res => {
        this.spinner = false;
        if (res && res.status == 1) {
          const targetIdx = this.notesList.map(item => item.noteId).indexOf(this.notesModel.noteId);
          this.notesList[targetIdx] = res.object;
          this.onClose();
        }
        else {
          this.spinner = false;
          alert('Error while updating note details')
        }
      }, error => {
        this.spinner = false;
        alert('Error while updating note details')
      });
    } else {
      this.notesModel.noteBackGroundColor = this.selectedColor;
      this.notesModel.noteForeGroundColor = '#fff';
      this.notesModel.userId = +localStorage.getItem("loggedInUserId");
      this.notes.saveNotes(this.notesModel).subscribe(res => {
        this.spinner = false;
        if (res && res.status == 1) {
          this.notesList.unshift(res.object);
          this.onClose();
        }
        else {
          this.spinner = false;
          alert('Error while updating note details')
        }
      }, error => {
        this.spinner = false;
        alert('Error while updating note details')
      });
    }
  }
  deleteNote(noteId: number) {
    this.notes.deleteNote(noteId).subscribe(res => {
      if (res && res.status == 1) {
        const targetIdx = this.notesList.map(item => item.noteId).indexOf(noteId);
        this.notesList.splice(targetIdx, 1);
        this.onClose();
      }
      else {
        alert('Error while updating note details')
      }
    }, error => {
      alert('Error while updating note details')
    });
  }
  logout() {
    this.loginService.logout();
    this.router.navigate(['/login']);
  }

}
