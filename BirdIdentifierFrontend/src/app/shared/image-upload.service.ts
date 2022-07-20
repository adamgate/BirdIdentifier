import { Injectable } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ImageUploadService {
  public birdName: string;

  public url: string = "https://localhost:44370/images";

  constructor(private http: HttpClient) { }

  //Send an image to the ML model for identification
  imageUpload(selectedFile: any) {
    const formData = new FormData();
    formData.append('image', selectedFile, selectedFile.name)

    this.http.post(this.url, formData, {
      reportProgress: true,
      observe: 'events'
    }).subscribe(
      event => {
        if (event.type === HttpEventType.UploadProgress) {
          console.log('Upload Progress: ' + Math.round(event.loaded / event.total * 100) + '%');
        }
        else if (event.type === HttpEventType.Response)
          console.log(event);
      });
  }
}
