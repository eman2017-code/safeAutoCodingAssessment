import { HttpClient, HttpEventType } from "@angular/common/http";
import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { map } from "rxjs/operators";
import { Trip } from "../../trip.model";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent {
  public message: string;
  public progress: number;
  @Output() public onUploadFinished = new EventEmitter();
  loadedTrips: Trip[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    //automatically list trips
    this.listTrips();
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      console.log("file length is 0");
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append("file", fileToUpload, fileToUpload.name);

    this.http
      .post("https://localhost:5001/api/trip", formData, {
        reportProgress: true,
        observe: "events",
      })
      .subscribe((event) => {
        if (event.type === HttpEventType.UploadProgress) {
          console.log("before");
          this.progress = Math.round((100 * event.loaded) / event.total);
        } else if (event.type === HttpEventType.Response) {
          console.log("after");
          this.message = "Upload success.";
          this.onUploadFinished.emit(event.body);
        }
      });
  };

  private listTrips() {
    //no subscription, no request
    this.http
      .get<{ [key: string]: Trip }>("https://localhost:5001/api/trip/listTrips")
      .pipe(
        map((responseData) => {
          console.log("responseData", responseData);
          const tripsArray: Trip[] = [];
          for (const key in responseData) {
            if (responseData.hasOwnProperty(key)) {
              tripsArray.push({ ...responseData[key] });
            }
          }
          console.log("tripsArray", tripsArray);
          return tripsArray;
        })
      )
      .subscribe((trips) => {
        this.loadedTrips = trips;
      });

    console.log("this.loadedTrips", this.loadedTrips);
  }
}
