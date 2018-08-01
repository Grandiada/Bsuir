import * as ko from "knockout"

export interface StartupData {
    clients: Client[];
    products: Product[];
}

export interface Client {
    id: number;
    name: string;
}

export interface Product {
    name: string;
    price: number;
}

export class ClientToAdd {
    public name = ko.observable<string>();
}

export class ProductToAdd {
    public name = ko.observable<string>();
    public price = ko.observable<number>();
}

export class ClientViewModel {
    readonly id: number;
    readonly name: string;

    constructor(client: Client) {
        this.id = client.id;
        this.name = client.name;
    }
    isSelected = ko.observable(false);
}

export class ProductViewModel {
    readonly name: string;
    readonly price: number;

    constructor(product: Product) {
        this.name = product.name;
        this.price = product.price;
    }
    isSelected = ko.observable(false);
}

