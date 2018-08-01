import { StartupData, ClientViewModel, ProductViewModel, ClientToAdd, ProductToAdd, Client, Product } from "./types";
import * as ko from "knockout";
import * as $ from "jquery";

export class Ui {

    _clients: KnockoutObservableArray<ClientViewModel>;
    _products: KnockoutObservableArray<ProductViewModel>;
    basket: KnockoutObservable<Basket>;

    clientToAdd = new ClientToAdd();
    productToAdd = new ProductToAdd();

    constructor(private startupData: StartupData) {
        if (startupData.clients.length > 0) {
            this._clients = ko.observableArray(startupData.clients.map(i => new ClientViewModel(i)));
            this.basket = ko.observable(new Basket(new ClientViewModel(startupData.clients[0])))

            if (startupData.products.length > 0)
                this._products = ko.observableArray(startupData.products.map(i => new ProductViewModel(i)));
            else
                this._products = ko.observableArray<ProductViewModel>();

        } else {
            this._clients = ko.observableArray<ClientViewModel>();
            this._products = ko.observableArray<ProductViewModel>();
            this.basket = ko.observable<Basket>(new Basket(null));
        }

    }

    addClient = (client: ClientToAdd) => {
        const clientName = client.name();
        if (clientName) {
            const data = { name: clientName };
            $.ajax({
                url: `/add-client/`,
                method: 'POST',
                data: data,

                success: (id: number) => {
                    let client: Client = {
                        id: id,
                        name: clientName
                    }
                    this._clients.push(new ClientViewModel(client))
                },
                error: error => {
                    const fullError = JSON.stringify(error);
                    console.log(fullError);
                }
            });
        }
    }

    buy = (basket: Basket) => {
        if (basket.products().length > 0) {
            const data = { id: basket.client().id, total: basket.total() };
            $.ajax({
                url: `/buy/`,
                method: 'POST',
                data: data,

                success: () => {
                    var client = this.basket().client();
                    this.basket().resetBasket(client);
                    alert("Товары отправлены");
                },
                error: error => {
                    const fullError = JSON.stringify(error);
                    console.log(fullError);
                }
            });
        }
    }

    addProduct = (product: ProductToAdd) => {
        const name = product.name();
        const price = product.price();

        if (name && price) {
            const data = { name: name, price: price };
            $.ajax({
                url: `/add-product/`,
                method: 'POST',
                data: data,

                success: () => {
                    let product: Product = {
                        name: name,
                        price: Number(price)
                    }
                    this._products.push(new ProductViewModel(product))
                },
                error: error => {
                    const fullError = JSON.stringify(error);
                    console.log(fullError);
                }
            });
        }
    }

    addToBasket = (product: ProductViewModel) => {
        this.basket().addBasket(product);
    }

    changeClient = (client: ClientViewModel) => {
        if (!this.basket())
            this.basket = ko.observable(new Basket(new ClientViewModel(client)))
        else
            this.basket().resetBasket(client);

    }
}

export class Basket {
    public client = ko.observable<ClientViewModel>();
    public name = ko.observable<string>();
    public products = ko.observableArray<ProductViewModel>();
    public discount = ko.observable<number>(0);

    public purchaseDisable = ko.pureComputed(() => {
        if (this.client().id == -1)
            return true;
        else
            return false;
    });

    public total = ko.pureComputed(() => {
        let price = 0;

        for (let item of this.products())
            price += item.price;

        return price
    });


    public totalWithDiscount = ko.pureComputed(() => {
        let price = 0;

        for (let item of this.products())
            price += item.price;
        const discount = price / 100 * this.discount();
        return price - discount
    });

    constructor(client: ClientViewModel | null) {
        if (client) {
            this.client(client);
            this.name(client.name);
            this.getDiscount(client.id);
        } else {
            let emptyClient: Client = { id: -1, name: "Выберите клиента" };
            this.client(new ClientViewModel(emptyClient));
            this.name(emptyClient.name);
        }
    }

    public resetBasket(client: ClientViewModel) {
        this.products.removeAll();
        this.client(client);
        this.name(client.name);
        this.getDiscount(client.id);
    }

    getDiscount = (clientId: number) => {
        if (clientId > 0) {
            const data = { id: clientId };
            $.ajax({
                url: `/get-discount/`,
                method: 'GET',
                data: data,

                success: (discount: number) => {
                    this.discount(discount);
                },
                error: error => {
                    const fullError = JSON.stringify(error);
                    console.log(fullError);
                }
            });
        }
    }

    public addBasket(product: ProductViewModel) {
        var copy = Object.assign({}, product);
        this.products.push(copy);
    }

    public removeFromBasket(product: ProductViewModel) {
        this.products.remove(product);
    }
}