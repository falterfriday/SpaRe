import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { Photo } from "./photo";
import { Injectable } from "@angular/core";
import "rxjs/add/operator/map";

@Injectable()
export class DataService {
	constructor(private http: Http) { }

	public photos: Photo[] = [];

	loadPhotos(): Observable<Photo[]> {
		return this.http.get("/api/photo").map((result: Response) => this.photos = result.json());
	}
}