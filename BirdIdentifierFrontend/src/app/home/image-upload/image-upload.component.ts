import { Component, OnInit } from '@angular/core';
import { empty } from 'rxjs';
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

    if (this.selectedFile) {
      return;
    }

    if (this.selectedFile)
    var fileExt = this.getExtension(this.selectedFile.name);

    if(this.isImage(fileExt)) {
      //enable upload button
      //set warning message to green and say "ready for upload"
    } else {
      //keep upload button disabled
      //show red warning message asking them to choose correct file type
    }
  }

  onUpload() {
    if (this.onFileSelected.length === 0)
      return;
    
    this.imageUploadService.imageUpload(<File>this.selectedFile);
  }

   getExtension(filename: string) {
    var parts = filename.split('.');
    return parts[parts.length - 1];
   }

   isImage(ext: string) {
    switch (ext.toLowerCase()) {
      case 'jpg':
      case 'jpeg':
      case 'png':
        //etc
        return true;
    }
    return false;
  }
}
