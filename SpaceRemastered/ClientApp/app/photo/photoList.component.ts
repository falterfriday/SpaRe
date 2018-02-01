import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Photo } from "../shared/photo";

@Component({
	selector: "photo-list",
	templateUrl: "photoList.component.html",
	styleUrls: ["photoList.component.css"]
})

export class PhotoList implements OnInit {
	public photos: Photo[];

	constructor(private data: DataService) {
		this.photos = data.photos;
	}

	ngOnInit(): void {
		this.data.loadPhotos().subscribe(() => this.photos = this.data.photos);
	}
}