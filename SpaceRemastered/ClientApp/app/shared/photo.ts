export interface Photo {
	id: number;
	title: string;
	description: string;
	height: number;
	width: number;
	url: string;
	hdUrl: string;
	favorites: number;
	photographer: string;
	views: number;
	thumbnail: string;
	retrievedFrom: string;
	downloadUrl: string;
	datePosted: Date;
	dateCreated: Date;
	dateUpdated: Date;
}