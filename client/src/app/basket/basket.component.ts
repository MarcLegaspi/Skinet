import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from './basket.service';
import { IBasket, IBasketItem, IBasketTotals } from '../shared/models/IBasket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket>;
  basketTotal$: Observable<IBasketTotals>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basketTotal$ = this.basketService.basketTotal$;
  }

  incrementItem(item: IBasketItem)
  {
    console.log(item);
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem)
  {
    console.log(item);
    this.basketService.decrementItemQuantity(item);
  }

  removeBasketItem(item: IBasketItem)
  {
    this.basketService.removeItemFromBasket(item);
  }
}
