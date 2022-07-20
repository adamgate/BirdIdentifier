import { Component, OnInit } from '@angular/core';
import { ImageUploadService } from 'src/app/shared/image-upload.service';

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent implements OnInit {
  selectedFile: File = null;

  constructor(private imageUploadService: ImageUploadService) { }

  ngOnInit(): void {
  }

  onFileSelected(event: any) {

    this.selectedFile = event.target.files[0];

    //Perform validation here
  }

  onUpload() {
    if (this.onFileSelected.length === 0)
      return;
    
    this.imageUploadService.imageUpload(<File>this.selectedFile);
  }
}
