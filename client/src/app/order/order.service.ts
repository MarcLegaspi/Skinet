import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { IOrder, IOrderItem } from '../shared/models/order';
import { IBasket, IBasketItem } from '../shared/models/IBasket';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getOrders() {
    return this.http.get<IOrder[]>(this.baseUrl + 'orders');
  }

  getOrder(id: number) {
    return this.http.get<IOrder>(this.baseUrl + 'orders/' + id);
  }

  mapOrderBasket(order: IOrder) {
    const basket: IBasket = {
      id: '',
      items: []
    };

    order.orderItems.forEach(orderItem => {
      basket.items.push({
        id: orderItem.productId,
        productName: orderItem.productName,
        price: orderItem.price,
        pictureUrl: orderItem.pictureUrl,
        quantity: orderItem.quantity,
        brand: '',
        type: ''
      });
    });

    return basket;
  }
}
